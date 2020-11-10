#region Licence

// Description: SymuBiz - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using Symu.SysDyn.Engine;
using Symu.SysDyn.Models;
using Symu.SysDyn.Models.XMile;

#endregion

namespace Symu.SysDyn.Parser
{
    /// <summary>
    ///     Parser for XML file following the XMILE standard for system dynamics
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

        public ModelCollection ParseModels()
        {
            if (_xDoc.Root == null)
            {
                throw new NullReferenceException(nameof(_xDoc.Root));
            }

            var models = _xDoc.Root.Elements(_ns + "model");

            var modelCollection = new ModelCollection();
            foreach (var model in models)
            {
                modelCollection.Add(ParseModel(model));
            }

            return modelCollection;
        }

        public XMileModel ParseModel(XElement xModel)
        {
            if (xModel == null)
            {
                throw new NullReferenceException(nameof(_xDoc.Root));
            }

            var variables = xModel.Descendants(_ns + "variables");

            var model = new XMileModel(xModel.FirstAttribute?.Value);
            foreach (var variable in variables)
            {
                ParseStocks(variable, model);
                ParseFlows(variable, model);
                ParseAuxiliaries(variable, model);
                ParseGroups(variable, model);
                ParseModules(variable, model);
            }

            return model;
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

        public void ParseAuxiliaries(XContainer variables, XMileModel model)
        {
            if (variables == null)
            {
                throw new ArgumentNullException(nameof(variables));
            }

            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            foreach (var auxiliary in variables.Descendants(_ns + "aux"))
            {
                Auxiliary.CreateInstance(auxiliary.FirstAttribute.Value,
                    model,
                    ParseEquation(auxiliary),
                    ParseGraphicalFunction(auxiliary),
                    ParseRange(auxiliary, "range"),
                    ParseRange(auxiliary, "scale"),
                    ParseNonNegative(auxiliary), ParseAccess(auxiliary));
            }
        }

        public void ParseFlows(XContainer variables, XMileModel model)
        {
            if (variables == null)
            {
                throw new ArgumentNullException(nameof(variables));
            }

            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            foreach (var flow in variables.Descendants(_ns + "flow"))
            {
                Flow.CreateInstance(flow.FirstAttribute.Value,
                    model,
                    ParseEquation(flow),
                    ParseGraphicalFunction(flow),
                    ParseRange(flow, "range"),
                    ParseRange(flow, "scale"),
                    ParseNonNegative(flow), ParseAccess(flow));
            }
        }

        public void ParseStocks(XContainer variables, XMileModel model)
        {
            if (variables == null)
            {
                throw new ArgumentNullException(nameof(variables));
            }

            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            foreach (var stock in variables.Descendants(_ns + "stock"))
            {
                Stock.CreateInstance(stock.FirstAttribute.Value,
                    model,
                    ParseEquation(stock),
                    ParseInflows(stock),
                    ParseOutflows(stock),
                    ParseGraphicalFunction(stock),
                    ParseRange(stock, "range"),
                    ParseRange(stock, "scale"),
                    ParseNonNegative(stock), ParseAccess(stock));
            }
        }


        public void ParseModules(XContainer variables, XMileModel model)
        {
            if (variables == null)
            {
                throw new ArgumentNullException(nameof(variables));
            }

            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            foreach (var module in variables.Descendants(_ns + "module"))
            {
                Module.CreateInstance(model,
                    module.FirstAttribute.Value,
                    ParseConnects(module));
            }
        }

        public ConnectCollection ParseConnects(XElement module)
        {
            if (module == null)
            {
                throw new ArgumentNullException(nameof(module));
            }

            var moduleName = module.FirstAttribute.Value;
            var xConnects = from q in module.Descendants(_ns + "connect")
                select new
                {
                    connectTo = q.FirstAttribute.Value,
                    connectFrom = q.LastAttribute.Value
                };
            var connects = new ConnectCollection();
            foreach (var connect in xConnects)
            {
                connects.Add(new Connect(moduleName, connect.connectTo, connect.connectFrom));
            }

            return connects;
        }

        public GraphicalFunction ParseGraphicalFunction(XContainer variable)
        {
            if (variable == null)
            {
                throw new ArgumentNullException(nameof(variable));
            }

            var gf = from q in variable.Descendants(_ns + "gf")
                select new
                {
                    xpts = q.Element(_ns + "xpts")?.Value,
                    ypts = q.Element(_ns + "ypts")?.Value,
                    xscale = GetScale(q, "xscale"),
                    yscale = GetScale(q, "yscale")
                };

            return gf.Select(value => new GraphicalFunction(value.xpts, value.ypts, value.xscale, value.yscale))
                .FirstOrDefault();
        }

        public List<string> ParseInflows(XContainer stock)
        {
            if (stock == null)
            {
                throw new ArgumentNullException(nameof(stock));
            }

            return stock.Elements(_ns + "inflow").Select(el => el.Value).ToList();
        }

        public List<string> ParseOutflows(XContainer stock)
        {
            if (stock == null)
            {
                throw new ArgumentNullException(nameof(stock));
            }

            return stock.Elements(_ns + "outflow").Select(el => el.Value).ToList();
        }

        public string ParseEquation(XContainer variable)
        {
            if (variable == null)
            {
                throw new ArgumentNullException(nameof(variable));
            }

            return variable.Element(_ns + "eqn")?.Value;
        }

        /// <summary>
        /// </summary>
        /// <param name="variable"></param>
        /// <param name="element">range or scale</param>
        /// <returns></returns>
        public Range ParseRange(XContainer variable, string element)
        {
            if (variable == null)
            {
                throw new ArgumentNullException(nameof(variable));
            }

            var range = variable.Descendants(_ns + element).FirstOrDefault();
            return range == null
                ? new Range()
                : new Range(range.Attribute("min")?.Value, range.Attribute("max")?.Value);
        }

        public NonNegative ParseNonNegative(XContainer variable)
        {
            if (variable == null)
            {
                throw new ArgumentNullException(nameof(variable));
            }

            var nonNegative = variable.Descendants(_ns + "non_negative").FirstOrDefault();
            return new NonNegative(nonNegative != null);
        }

        public static VariableAccess ParseAccess(XElement variable)
        {
            if (variable == null)
            {
                throw new ArgumentNullException(nameof(variable));
            }

            var access = variable.Attribute("access");
            if (access == null)
            {
                return VariableAccess.None;
            }

            switch (access.Value)
            {
                case "input":
                    return VariableAccess.Input;
                case "output":
                    return VariableAccess.Output;
                default:
                    throw new ArgumentOutOfRangeException();
            }
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

        public void ParseGroups(XContainer variables, XMileModel model)
        {
            if (variables == null)
            {
                throw new ArgumentNullException(nameof(variables));
            }

            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var groups = variables.Descendants(_ns + "group")
                .Select(q => new Group(
                    q.FirstAttribute.Value,
                    model.Name,
                    ParseEntities(q)
                ));

            model.Groups.AddRange(groups);
        }

        public List<string> ParseEntities(XContainer xContainer)
        {
            if (xContainer == null)
            {
                throw new ArgumentNullException(nameof(xContainer));
            }

            return xContainer.Elements(_ns + "entity").Select(entity => entity.Attribute("name")?.Value).ToList();
        }
    }
}