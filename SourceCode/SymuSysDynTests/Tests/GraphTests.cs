using System;
using System.IO;
using System.Linq;
using SystemAnalyzer.Graphs;
using SystemAnalyzer.Graphs.Parsing;
using NUnit.Framework;

namespace SystemAnalyzer.Tests
{
	internal class GraphTests
	{
		[Test, TestCase("teacup", 2, 1), TestCase("Borneo", 19, 23)]
		public void GraphParsingTest(string filename, int expectedVertices, int expectedEdges)
		{
			string filepath = AppDomain.CurrentDomain.BaseDirectory + @"..\..\Templates\" + filename + ".xmile";
			var parser = new GraphParser(filepath, false);
			var graph  = parser.CreateGraph("DEFAULT");

			Assert.AreEqual(graph.VertexCount, expectedVertices);
			Assert.AreEqual(graph.EdgeCount, expectedEdges);
		}

		[Test]
		public void GraphParserExceptionsTest()
		{
			Assert.Throws<ArgumentException>(() => new GraphParser(@"/\|*:?"));

			Assert.Throws<FileNotFoundException>(() => new GraphParser("Non-existant file!"));
		}

	    [Test]
	    public void ResultTest()
	    {
	        var graph = new Graph(true, new Stock("[GLOBAL]"));
	        var stocks = Enumerable.Range(0, 'O' - 'A' + 1)
	                               .Select(c => new Stock(((char) (c + 'A')).ToString()))
	                               .ToArray();
	        graph.AddVertexRange(stocks);
	        void makeFlow(char a, char b)
	        {
	            var name = a.ToString() + b.ToString();
	            var flow = new FlowEdge(name, stocks[a - 'A'], stocks[b - 'A']);
	            graph.AddEdge(flow);
	        }

	        makeFlow('B', 'C');
	        makeFlow('B', 'J');
	        makeFlow('C', 'A');
	        makeFlow('D', 'C');
	        makeFlow('D', 'H');
	        makeFlow('D', 'I');
	        makeFlow('I', 'H');
	        makeFlow('J', 'H');
	        makeFlow('L', 'N');
	        makeFlow('N', 'M');
	        makeFlow('A', 'O');
	        makeFlow('B', 'O');
	        makeFlow('D', 'E');
	        makeFlow('E', 'G');
	        makeFlow('G', 'D');
	        makeFlow('D', 'F');
	        makeFlow('E', 'F');
	        makeFlow('G', 'F');
	        makeFlow('I', 'J');
	        makeFlow('I', 'M');
	        makeFlow('J', 'L');
	        makeFlow('L', 'I');
	        makeFlow('I', 'K');
	        makeFlow('J', 'K');
	        makeFlow('L', 'K');

            Program.Process(graph);
	    }

	    [Test]
	    public void SmallResultTest()
	    {
	        var graph = new Graph(true, new Stock("[GLOBAL]"));
	        var stocks = Enumerable.Range(0, 'G' - 'A' + 1)
	                               .Select(c => new Stock(((char) (c + 'A')).ToString()))
	                               .ToArray();
	        graph.AddVertexRange(stocks);
	        void makeFlow(char a, char b)
	        {
	            var name = a.ToString() + b.ToString();
	            var flow = new FlowEdge(name, stocks[a - 'A'], stocks[b - 'A']);
	            graph.AddEdge(flow);
	        }

	        makeFlow('A', 'B');
	        makeFlow('B', 'C');
	        makeFlow('C', 'A');

	        makeFlow('D', 'E');
	        makeFlow('E', 'F');
	        makeFlow('F', 'D');

	        makeFlow('A', 'D');
	        makeFlow('F', 'G');
            makeFlow('G', 'C');

	        Program.Process(graph);
	    }
	}
}
