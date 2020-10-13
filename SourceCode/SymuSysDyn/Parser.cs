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
using Symu.SysDyn.QuickGraph;
using Symu.SysDyn.Simulation;

#endregion

namespace Symu.SysDyn
{
    public class Parser
    {
        //todo put in a resource
        private const string SchemaLocation = @"C:\Users\laure\Dropbox\Symu\SourceCode\Symu.SysDyn\Github\SourceCode\SymuSysDyn\Templates\schema.xsd";

        private readonly string[] _fileExtensions = {".xmile", ".xml", ".stmx"};
        private readonly XDocument _xDoc;

        public Parser(string xmlFile, bool validate = true)
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

            XNamespace ns = _xDoc.Root.Attributes("xmlns").First().Value;
            var xElements = _xDoc.Root.Descendants(ns + "variables");

            var nodes = new Variables();
            foreach (var xElement in xElements)
            {
                ParseStocks(xElement, ns, nodes);
                ParseFlows(xElement, ns, nodes);
                ParseAuxiliaries(xElement, ns, nodes);
            }
            return nodes;
        }
        public SimSpecs ParseSimSpecs()
        {
            if (_xDoc.Root == null)
            {
                throw new NullReferenceException(nameof(_xDoc.Root));
            }

            XNamespace ns = _xDoc.Root.Attributes("xmlns").First().Value;
            var sim = _xDoc.Root.Element(ns + "sim_specs");
            if (sim == null)
            {
                return new SimSpecs(0, 0, 1);
            }
            var start = float.Parse(sim.Element(ns + "start")?.Value ?? "0", CultureInfo.InvariantCulture);
            var stop = float.Parse(sim.Element(ns + "stop")?.Value ?? "0", CultureInfo.InvariantCulture);
            var dt = float.Parse(sim.Element(ns + "dt")?.Value ?? "1", CultureInfo.InvariantCulture);
            return new SimSpecs(start, stop, dt);
        }

        public static Graph CreateGraph(Variables variables)
        {
            if (variables == null)
            {
                throw new ArgumentNullException(nameof(variables));
            }
            var graph = new Graph(true);
            graph.AddVertexRange(variables.Select(x => x.Name).ToList());
            foreach (var variable in variables)
            {
                foreach (var edge in variable.Children.Select(child => new VariableEdge(child, variable.Name)))
                {
                    graph.AddEdge(edge);
                }
            }
            return graph;
        }

        private static void ParseAuxiliaries(XContainer xContainer, XNamespace ns, Variables nodes)
        {
            var auxiliaries = xContainer.Descendants(ns + "aux")
                .Select(q => new Auxiliary(
                    q.FirstAttribute.Value,
                    q.Element(ns + "eqn")?.Value,
                    CreateTable(q, ns)));

            nodes.AddRange(auxiliaries);
        }

        private static void ParseFlows(XContainer xContainer, XNamespace ns, Variables nodes)
        {
            var flows = xContainer.Descendants(ns + "flow")
                .Select(q => new Flow(
                    q.FirstAttribute.Value,
                    q.Element(ns + "eqn")?.Value));

            nodes.AddRange(flows);
        }

        private static void ParseStocks(XContainer xContainer, XNamespace ns, Variables nodes)
        {
            var stocks = xContainer.Descendants(ns + "stock")
                .Select(q => new Stock(
                    q.FirstAttribute.Value,
                    q.Element(ns + "eqn")?.Value,
                    q.Elements(ns + "inflow").Select(el => el.Value).ToList(),
                    q.Elements(ns + "outflow").Select(el => el.Value).ToList()
                ));

            nodes.AddRange(stocks);
        }

        private static GraphicalFunction CreateTable(XContainer element, XNamespace ns)
        {
            var table = from q in element.Descendants(ns + "gf")
                select new
                {
                    list = q.Element(ns + "ypts")?.Value,
                    bounds = GetBounds(q)
                };

            return table.Select(value => new GraphicalFunction(value.list, value.bounds)).FirstOrDefault();
        }

        private static string[] GetBounds(XContainer element)
        {
            var bounds = new string[4];
            var elements = element.Elements().ToList();

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