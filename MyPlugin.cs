using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Reflection;
using XrmToolBox.Extensibility;
using XrmToolBox.Extensibility.Interfaces;

namespace SolutionComponents
{
    // MEF plugin metadata definition for XrmToolBox Tool Library (G-5)
    [Export(typeof(IXrmToolBoxPlugin)),
        ExportMetadata("Name", "Solution Component Inspector"),
        ExportMetadata("Description", "Analyze solution component dependencies and historical releases in Dynamics 365 / Dataverse"),
        ExportMetadata("SmallImageBase64", "iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAC1SURBVFhHY2AYDEDdPd5B3TW+ge7YPd4B4gDX+AYNt/j/9MYge1EcYBiQ8d8yopDmGGQPVgeAJL2ym2iOQfaMOmDoOCCwsANDjBSMTT9JDgDhZx9+kY3RzaK6A1rnrscQo6sDiJFHx6MOGHUAWQ4o6lmIIY4Pg9RT1QG4MDHy6HjUAaMOoKoD4momYojR1QGEMLpZJDvAJ7cFQ4wUjE0/SQ6gBR51wOB1wID3jOiNEQ4YwN4xAF8t2xsDmXVAAAAAAElFTkSuQmCC"),
        ExportMetadata("BigImageBase64", "iVBORw0KGgoAAAANSUhEUgAAAFAAAABQCAYAAACOEfKtAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAFjSURBVHhe7d0xSgNBGEDh3Ca2lnbbxAWx0M7KaQWxSGqLIAoWNh7AwhN4K+vUaSKDhWTHEHfeju7gG/iaf2H4eQeYnUw8/Ezb0ExnYame2tB8BpyF5cFx2Kif2G0r4OHZ1eboYq49YqdvA8aPJ9d32iN2MiBgQMiAkAEhA0IGhAwIGRAyIGRAyICQASEDQgaEDAgVDXg+f0xmYzLEfkUDRu+r9Wh1d81RVcCHl7dkRnR3zVFVwBL3UQaEDAgZEDIgZEDIgJABIQNCBoQMCBkQMiBkQMiAkAEhA0K/EnDx9JrM/1Lcp6qAQylxH2VAyICQASEDQgaEDAgZEDIgZEDIgJABIQNCBoQMCBkQMiBUVcDL2+dkRnR3zVFVwKF1d81RNODpzX0yG5Mh9isa8D8wIGRAyICQASEDQgaEDAgZEDIgZEDIgJABIQNCBoQMCO0M6EvmP7PzJXP18xXQvznkaUPzAWQnGlaDBCEkAAAAAElFTkSuQmCC"),
        ExportMetadata("BackgroundColor", "Lavender"),
        ExportMetadata("PrimaryFontColor", "Black"),
        ExportMetadata("SecondaryFontColor", "DarkSlateGray")]
    public class MyPlugin : PluginBase
    {
        public override IXrmToolBoxPluginControl GetControl()
        {
            return new MyPluginControl();
        }

        public MyPlugin()
        {
        }
    }
}