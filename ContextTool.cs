//------------------------------------------------------------------------------
// <copyright file="ContextTool.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace DCBrowser
{
    using System;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Shell;

    /// <summary>
    /// This class implements the tool window exposed by this package and hosts a user control.
    /// </summary>
    /// <remarks>
    /// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane,
    /// usually implemented by the package implementer.
    /// <para>
    /// This class derives from the ToolWindowPane class provided from the MPF in order to use its
    /// implementation of the IVsUIElementPane interface.
    /// </para>
    /// </remarks>
    [Guid("7f0d7af1-28d0-49c3-96de-a9ee7d42cce7")]
    public class ContextTool : ToolWindowPane
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContextTool"/> class.
        /// </summary>
        public ContextTool() : base(null)
        {
            this.Caption = "Browsing Tool";

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.
            this.Content = new ContextToolControl();
        }
    }
}
