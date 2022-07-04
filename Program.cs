using NamespaceSort;

string program =
"""
using System;
using System.IO;
using Microsoft.CSharp;
using TestNamespace;
using System.Collections.Generic;
using System.Collections.Linq;
""";

var input = "System.* ; * ; TestNamespace ; System.Collections.*";
var test = new NamespaceImportParser().Parse(program);
var result = new NamespaceGroupWriter(new NamespaceSorter(new NamespaceSortConfigParser().Parse(input)).Sort(test));

Console.Write(result.Write());

