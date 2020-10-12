using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;

namespace SymuSysDynApp.Graph
{
    public static class GraphViz
    {
        public const int Success = 0;
        public static Image RenderImage(string source, string format)
        {
            // Create a Graphviz context
            var gvc = NativeMethods.gvContext();
            if (gvc == IntPtr.Zero)
                throw new Exception("Failed to create Graphviz context.");

            // Load the DOT data into a graph
            var g = NativeMethods.agmemread(source);
            if (g == IntPtr.Zero)
                throw new Exception("Failed to create graph from source. Check for syntax errors.");

            // Apply a layout
            if (NativeMethods.gvLayout(gvc, g, "dot") != Success)
                throw new Exception("Layout failed.");

            // Render the graph
            if (NativeMethods.gvRenderData(gvc, g, format, out var result, out var length) != Success)
                throw new Exception("Render failed.");

            // Create an array to hold the rendered graph
            var bytes = new byte[length];

            // Copy the image from the IntPtr
            Marshal.Copy(result, bytes, 0, length);

            // Free up the resources
            _ = NativeMethods.gvFreeRenderData(result);
            _ = NativeMethods.gvFreeLayout(gvc, g);
            NativeMethods.agclose(g);
            _ = NativeMethods.gvFreeContext(gvc);
            using (var stream = new MemoryStream(bytes))
            {
                return Image.FromStream(stream);
            }
        }
    }
}
