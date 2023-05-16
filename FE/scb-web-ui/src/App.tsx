import React from "react";
import { useState } from "react";
import "./App.scss";
import { Link, Outlet, NavLink } from "react-router-dom";
import Container from "react-bootstrap/Container";
import Nav from "react-bootstrap/Nav";
import Navbar from "react-bootstrap/Navbar";
import NavDropdown from "react-bootstrap/NavDropdown";

export const loader = async () => {
  // const contacts: any = await getContacts();
  // return { contacts };
};

function App() {
  const [count, setCount] = useState(0);

  return (
    <>
      <Navbar expand="lg" className="nav-bar">
        <Container>
          <Navbar.Brand>
            <NavLink
              to={"/info"}
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
                  Home
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
