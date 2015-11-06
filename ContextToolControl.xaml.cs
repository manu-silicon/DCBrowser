//------------------------------------------------------------------------------
// <copyright file="ContextToolControl.xaml.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.VisualStudio.Settings.Internal;

namespace DCBrowser
{
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for ContextToolControl.
    /// </summary>
    public partial class ContextToolControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContextToolControl"/> class.
        /// </summary>
        public ContextToolControl()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Handles click on the button by displaying a message box.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>
        [SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions", Justification = "Sample code")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Default event handler naming pattern")]
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                string.Format(System.Globalization.CultureInfo.CurrentUICulture, "Invoked '{0}'", this.ToString()),
                "ContextTool");
        }

        private void OnClassNameKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (ClassName.Text != null)
            {
                LoadSolution();
            }

        }

        private void OnMemberNameKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (MemberName.Text != null)
            {
            }

        }

        public void LoadSolution()
        {
            var workspace = MSBuildWorkspace.Create();
            var solution = workspace.OpenSolutionAsync("C:\\work\\Projects\\WCL\\WCL.sln").Result;
            var sortedProject = solution.GetProjectDependencyGraph().GetTopologicallySortedProjects().Select(solution.GetProject);

            foreach (var project in sortedProject)
            {
                var compilation = project.GetCompilationAsync().Result;
                var diag1 = compilation.GetDiagnostics();
                foreach (
                    var l_class in compilation.GlobalNamespace.GetNamespaceMembers().SelectMany(x => x.GetMembers()))
                {
                    BrowsingTree.Items.Add(new TreeViewItem() {Header = l_class.Name});
                }
                var classVisitor = new ClassVirtualizationVisitor();

                foreach (var syntaxTree in compilation.SyntaxTrees)
                {
                    classVisitor.Visit(syntaxTree.GetRoot());
                }

                var classes = classVisitor.Classes;
            }
        }

        class ClassVirtualizationVisitor : CSharpSyntaxRewriter
        {
            public ClassVirtualizationVisitor()
            {
                Classes = new List<ClassDeclarationSyntax>();
            }

            public List<ClassDeclarationSyntax> Classes { get; set; }

            public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax node)
            {
                node = (ClassDeclarationSyntax)base.VisitClassDeclaration(node);
                Classes.Add(node); // save your visited classes
                return node;
            }
        }
    }
}