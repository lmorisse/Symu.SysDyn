#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using Symu.SysDyn.Model;
using Symu.SysDyn.Simulation;

#endregion

namespace Symu.SysDyn.Parser
{
    /// <summary>
    /// Parser for XML file following the XMILE standard for system dynamics
    /// </summary>
    /// <remarks>http://docs.oasis-open.org/xmile/xmile/v1.0/xmile-v1.0.html</remarks>
    public class XmlParser
    {
        //todo put in a resource

        public const string SchemaLocation =
            @"C:\Users\laure\Dropbox\Symu\SourceCode\Symu.SysDyn\Github\SourceCode\SymuSysDyn\Templates\schema.xsd";

        //todo put in a resource
        private readonly string[] _fileExtensions = {".xmile", ".xml", ".stmx", ".itmx"};
        private readonly XNamespace _ns;
        private readonly XDocument _xDoc;

        public XmlParser(string xmlFile, bool validate = true)
        {
            var file = new FileInfo(xmlFile);
            if (!file.Exists)
            {
                throw new FileNotFoundException("File not found");
            }

            if (!_fileExtensions.Contains(file.Extension))
            {
                throw new ArgumentException("File extension must be .xmile, .xml, .stmx or .itmx");
            }

            _xDoc = XDocument.Load(xmlFile);
            _ns = _xDoc.Root?.Attributes("xmlns").First().Value;

            // Do not validate a .stmx file

            if (!validate)
            {
                return;
            }

            //todo: check if schema exists
            using (var reader = new StreamReader(SchemaLocation))
            using (var xmlReader = XmlReader.Create(reader))
            {
                var schemas = new XmlSchemaSet();
                var schema = XmlSchema.Read(xmlReader, null);

                schemas.Add(schema);
                _xDoc.Validate(schemas, (sender, e) =>
                {
                    if (e.Exception == null)
                    {
                        return;
                    }

                    throw new XmlSchemaValidationException("Erreur de validation:\n" + e.Message,
                        e.Exception);
                });
            }
        }

        public Variables ParseVariables()
        {
            if (_xDoc.Root == null)
            {
                throw new NullReferenceException(nameof(_xDoc.Root));
            }

            var xElements = _xDoc.Root.Descendants(_ns + "variables");

            var variables = new Variables();
            foreach (var xElement in xElements)
            {
                ParseStocks(xElement, variables);
                ParseFlows(xElement, variables);
                ParseAuxiliaries(xElement, variables);
            }

            return variables;
        }

        public SimSpecs ParseSimSpecs()
        {
            if (_xDoc.Root == null)
            {
                throw new NullReferenceException(nameof(_xDoc.Root));
            }

            var sim = _xDoc.Root.Element(_ns + "sim_specs");
            if (sim == null)
            {
                return new SimSpecs(0, 0, 1);
            }

            var start = sim.Element(_ns + "start")?.Value;
            var stop = sim.Element(_ns + "stop")?.Value;
            var dt = sim.Element(_ns + "dt")?.Value;
            var pause = sim.Attribute("pause")?.Value;
            var timeUnits = sim.Attribute("time_units")?.Value;
            return new SimSpecs(start, stop, dt, pause, timeUnits);
        }

        public void ParseAuxiliaries(XContainer xContainer, Variables variables)
        {
            if (xContainer == null)
            {
                throw new ArgumentNullException(nameof(xContainer));
            }

            if (variables == null)
            {
                throw new ArgumentNullException(nameof(variables));
            }

            var auxiliaries = xContainer.Descendants(_ns + "aux")
                .Select(q => new Auxiliary(
                    q.FirstAttribute.Value,
                    ParseEquation(q),
                    ParseGraphicalFunction(q),
                    ParseRange(q),
                    ParseScale(q)));

            variables.AddRange(auxiliaries);
        }

        public void ParseFlows(XContainer xContainer, Variables variables)
        {
            if (xContainer == null)
            {
                throw new ArgumentNullException(nameof(xContainer));
            }

            if (variables == null)
            {
                throw new ArgumentNullException(nameof(variables));
            }

            var flows = xContainer.Descendants(_ns + "flow")
                .Select(q => new Flow(
                    q.FirstAttribute.Value,
                    ParseEquation(q),
                    ParseGraphicalFunction(q),
                    ParseRange(q),
                    ParseScale(q)));

            variables.AddRange(flows);
        }

        public void ParseStocks(XContainer xContainer, Variables variables)
        {
            if (xContainer == null)
            {
                throw new ArgumentNullException(nameof(xContainer));
            }

            if (variables == null)
            {
                throw new ArgumentNullException(nameof(variables));
            }

            var stocks = xContainer.Descendants(_ns + "stock")
                .Select(q => new Stock(
                    q.FirstAttribute.Value,
                    ParseEquation(q),
                    ParseInflow(q),
                    ParseOutflow(q),
                    ParseGraphicalFunction(q),
                    ParseRange(q),
                    ParseScale(q)
                ));

            variables.AddRange(stocks);
        }

        public GraphicalFunction ParseGraphicalFunction(XContainer xContainer)
        {
            if (xContainer == null)
            {
                throw new ArgumentNullException(nameof(xContainer));
            }

            var gf = from q in xContainer.Descendants(_ns + "gf")
                select new
                {
                    xpts = q.Element(_ns + "xpts")?.Value,
                    ypts = q.Element(_ns + "ypts")?.Value,
                    xscale = GetScale(q, "xscale"),
                    yscale = GetScale(q, "yscale")
                };

            return gf.Select(value => new GraphicalFunction(value.xpts, value.ypts, value.xscale, value.yscale)).FirstOrDefault();
        }

        public List<string> ParseInflow(XContainer xContainer)
        {
            if (xContainer == null)
            {
                throw new ArgumentNullException(nameof(xContainer));
            }

            return xContainer.Elements(_ns + "inflow").Select(el => el.Value).ToList();
        }

        public List<string> ParseOutflow(XContainer xContainer)
        {
            if (xContainer == null)
            {
                throw new ArgumentNullException(nameof(xContainer));
            }

            return xContainer.Elements(_ns + "outflow").Select(el => el.Value).ToList();
        }

        public string ParseEquation(XContainer xContainer)
        {
            if (xContainer == null)
            {
                throw new ArgumentNullException(nameof(xContainer));
            }

            return xContainer.Element(_ns + "eqn")?.Value;
        }

        public Range ParseRange(XContainer xContainer)
        {
            if (xContainer == null)
            {
                throw new ArgumentNullException(nameof(xContainer));
            }

            var range = xContainer.Descendants(_ns + "range").FirstOrDefault();
            return range == null ? new Range(false) : new Range(range.Attribute("min")?.Value, range.Attribute("max")?.Value, false);
        }

        public Range ParseScale(XContainer xContainer)
        {
            if (xContainer == null)
            {
                throw new ArgumentNullException(nameof(xContainer));
            }

            var range = xContainer.Descendants(_ns + "scale").FirstOrDefault();
            var nonNegative = xContainer.Descendants(_ns + "non_negative").FirstOrDefault();
            return range == null ? new Range(nonNegative != null) : new Range(range.Attribute("min")?.Value, range.Attribute("max")?.Value, nonNegative != null);
        }


        private string[] GetScale(XContainer xContainer, string scale)
        {
            var element = xContainer?.Element(_ns + scale);

            if (element == null)
            {
                return null;
            }
            var getScale = new string[2];

            getScale[0] = element.Attribute("min")?.Value;
            getScale[1] = element.Attribute("max")?.Value;

            return getScale;
        }
    }
}