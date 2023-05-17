import "./App.scss";
import { Outlet, NavLink } from "react-router-dom";
import Container from "react-bootstrap/Container";
import Nav from "react-bootstrap/Nav";
import Navbar from "react-bootstrap/Navbar";

function App() {
  return (
    <>
      <Navbar expand="lg" className="nav-bar">
        <Container>
          <Navbar.Brand>
            <NavLink
              to={"/"}
              className={({ isActive }) =>
                isActive ? "nav-brand" : "nav-brand"
              }
            >
              Scb Project
            </NavLink>
          </Navbar.Brand>
          <Navbar.Toggle aria-controls="basic-navbar-nav" />
          <Navbar.Collapse id="basic-navbar-nav">
            <Nav className="me-auto">
              <Nav.Link>
                <NavLink
                  to={"/info"}
                  className={({ isActive }) =>
                    isActive ? "active" : "nav-item"
                  }
                >
                  Info
                </NavLink>
              </Nav.Link>
              <Nav.Link>
                <NavLink
                  to={`/statistics`}
                  className={({ isActive }) =>
                    isActive ? "active" : "nav-item"
                  }
                >
                  Statistics
                </NavLink>
              </Nav.Link>
            </Nav>
          </Navbar.Collapse>
        </Container>
      </Navbar>
      <div id="main-container">
        <Outlet />
      </div>
    </>
  );
}

export default App;
