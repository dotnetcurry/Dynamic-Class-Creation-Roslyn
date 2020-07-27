using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace DynamicClassCreation
{
    /// <summary>
    /// This class uses the Roslyn CaaS features to obtain C# source code from source files
    /// stored in the hard disk
    /// NOTE: if source code is not available, a decompiler such as dotPeek could be used instead
    /// </summary>
    class SourceCodeFromFile
    {
        private readonly SyntaxTree _syntaxTree;

        /// <summary>
        /// Path of the source code file to analyze
        /// </summary>
        /// <param name="path"></param>
        public SourceCodeFromFile(string path)
        {
            using var stream = File.OpenRead(path);
            _syntaxTree = CSharpSyntaxTree.ParseText(SourceText.From(stream), path: path);
        }

        /// <summary>
        /// Gets the source code of a property getter
        /// </summary>
        /// <param name="propertyName">Property name</param>
        /// <returns>Source code</returns>
        public string GetPropertyGetterSourceCode(string propertyName)
        {
            var root = _syntaxTree.GetRoot();
            var property = root
                .DescendantNodes()
                .OfType<PropertyDeclarationSyntax>()
                .FirstOrDefault(md => md.Identifier.ValueText.Equals(propertyName));
            var getter = property?.AccessorList.Accessors.First(a => a.IsKind(SyntaxKind.GetAccessorDeclaration));
            return getter?.ToString();
        }

        /// <summary>
        /// Gets the source code of a property setter
        /// </summary>
        /// <param name="propertyName">Property name</param>
        /// <returns>Source code</returns>
        public string GetPropertySetterSourceCode(string propertyName)
        {
            var root = _syntaxTree.GetRoot();
            var property = root
                .DescendantNodes()
                .OfType<PropertyDeclarationSyntax>()
                .FirstOrDefault(md => md.Identifier.ValueText.Equals(propertyName));
            var setter = property?.AccessorList.Accessors.First(a => a.IsKind(SyntaxKind.SetAccessorDeclaration));
            return setter?.ToString();
        }

        /// <summary>
        /// Gets the source code of a method (method body only, including {})
        ///
        /// NOTE: This is a demo that only obtains the first method whose name matches. Overloads
        /// are not considered.
        /// </summary>
        /// <param name="methodName">Method name</param>
        /// <returns>Method body</returns>
        public string GetMethodSourceCode(string methodName)
        {
            var root = _syntaxTree.GetRoot();
            var method = root
                .DescendantNodes()
                .OfType<MethodDeclarationSyntax>()
                .FirstOrDefault(md => md.Identifier.ValueText.Equals(methodName));
            return method?.Body.ToString();
        }

        /// <summary>
        /// Gets the signature of a method (header only, not body)
        /// NOTE: This is a demo that only obtains the first method whose name matches. Overloads
        /// are not considered.
        /// </summary>
        /// <param name="methodName">Method name</param>
        /// <returns>Method signature</returns>
        public string GetMethodSignature(string methodName)
        {
            var root = _syntaxTree.GetRoot();
            var method = root
                .DescendantNodes()
                .OfType<MethodDeclarationSyntax>()
                .FirstOrDefault(md => md.Identifier.ValueText.Equals(methodName));
            return method?.ToString().Split('{')[0];
        }
    }
}
