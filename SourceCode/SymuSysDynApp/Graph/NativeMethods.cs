#region Licence

// Description: SymuSysDyn - SymuSysDynApp
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

using System;
using System.Runtime.InteropServices;

namespace SymuSysDynApp.Graph
{
    internal static class NativeMethods
    {
        public const string LibGvc = @".\external\gvc.dll";
        public const string LibGraph = @".\external\cgraph.dll";
        /// 
        /// Creates a new Graphviz context.
        /// 

        [DllImport(LibGvc, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr gvContext();

        /// 
        /// Releases a context's resources.
        /// 
        [DllImport(LibGvc, CallingConvention = CallingConvention.Cdecl)]
        public static extern int gvFreeContext(IntPtr gvc);

        /// 
        /// Reads a graph from a string.
        /// 
        [DllImport(LibGraph, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr agmemread(string data);


        /// 
        /// Releases the resources used by a graph.
        /// 
        [DllImport(LibGraph, CallingConvention = CallingConvention.Cdecl)]
        public static extern void agclose(IntPtr g);

        /// 
        /// Applies a layout to a graph using the given engine.
        /// 
        [DllImport(LibGvc, CallingConvention = CallingConvention.Cdecl)]
        public static extern int gvLayout(IntPtr gvc, IntPtr g, string engine);


        /// 
        /// Releases the resources used by a layout.
        /// 
        [DllImport(LibGvc, CallingConvention = CallingConvention.Cdecl)]
        public static extern int gvFreeLayout(IntPtr gvc, IntPtr g);

        /// 
        /// Renders a graph to a file.
        /// 
        [DllImport(LibGvc, CallingConvention = CallingConvention.Cdecl)]
        public static extern int gvRenderFilename(IntPtr gvc, IntPtr g,
            string format, string fileName);

        /// 
        /// Renders a graph in memory.
        /// 
        [DllImport(LibGvc, CallingConvention = CallingConvention.Cdecl)]
        public static extern int gvRenderData(IntPtr gvc, IntPtr g,
            string format, out IntPtr result, out int length);

        /// 
        /// Release render resources.
        /// 
        [DllImport(LibGvc, CallingConvention = CallingConvention.Cdecl)]
        public static extern int gvFreeRenderData(IntPtr result);
    }
}