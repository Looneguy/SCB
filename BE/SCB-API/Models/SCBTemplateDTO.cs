using System;
using System.Collections.Generic;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Globalization;

namespace SCB_API.Models
{
    public class SCBTemplateDTO
    {
            public string Title { get; set; }
            public Variable[]Variables { get; set; }
    }

    public class Variable
    {
        public string Code { get; set; }
        public string Text { get; set; }
        public string[] Values { get; set; }
        public string[] ValueTexts { get; set; }
        public bool? Elimination { get; set; }
        public bool? Time { get; set; }
    }
}
