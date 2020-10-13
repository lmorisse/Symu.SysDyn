#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
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
    public class XmlParser
    {
        //todo put in a resource
        private readonly string[] _fileExtensions = {".xmile", ".xml", ".stmx"};
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
                throw new ArgumentException("File extension must be .xmile, .xml or .stmx");
            }

            _xDoc = XDocument.Load(xmlFile);
            _ns = _xDoc.Root?.Attributes("xmlns").First().Value;

            // Do not validate a .stmx file

            if (!validate)
            {
                return;
            }

            //todo: check if schema exists
            using (var reader = new StreamReader(XmlConstants.SchemaLocation))
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

            var start = float.Parse(sim.Element(_ns + "start")?.Value ?? "0", CultureInfo.InvariantCulture);
            var stop = float.Parse(sim.Element(_ns + "stop")?.Value ?? "0", CultureInfo.InvariantCulture);
            var dt = float.Parse(sim.Element(_ns + "dt")?.Value ?? "1", CultureInfo.InvariantCulture);
            return new SimSpecs(start, stop, dt);
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
                    q.Element(_ns + "eqn")?.Value,
                    ParseGraphicalFunction(q)));

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
                    q.Element(_ns + "eqn")?.Value));

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
                    q.Element(_ns + "eqn")?.Value,
                    q.Elements(_ns + "inflow").Select(el => el.Value).ToList(),
                    q.Elements(_ns + "outflow").Select(el => el.Value).ToList()
                ));

            variables.AddRange(stocks);
        }

        public GraphicalFunction ParseGraphicalFunction(XContainer xContainer)
        {
            if (xContainer == null)
            {
                throw new ArgumentNullException(nameof(xContainer));
            }

            var ypts = from q in xContainer.Descendants(_ns + "gf")
                select new
                {
                    list = q.Element(_ns + "ypts")?.Value,
                    bounds = GetBounds(q)
                };

            return ypts.Select(value => new GraphicalFunction(value.list, value.bounds)).FirstOrDefault();
        }

        private static string[] GetBounds(XContainer xContainer)
        {
            var bounds = new string[4];
            if (xContainer == null)
            {
                return bounds;
            }

            var elements = xContainer.Elements().ToList();

            var xLower = elements.First().Attribute("min")?.Value;
            var xUpper = elements.First().Attribute("max")?.Value;
            var yLower = elements.ElementAt(1).Attribute("min")?.Value;
            var yUpper = elements.ElementAt(1).Attribute("max")?.Value;

            bounds[0] = xLower;
            bounds[1] = xUpper;
            bounds[2] = yLower;
            bounds[3] = yUpper;

            return bounds;
        }
    }
}