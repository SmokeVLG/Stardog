
dotNetRDF Stardog Helper
=============================================================
This class acts as helper functions to let you quickly communicate with Stardog without having
to learn a whole lot about dotNetRDF.

To set up it you just need to update the first few functions to define
the data connection string, user name nad password.

One of the more useful things it does is automatically prefix your queries with some standard
SPARQL prefixes:

	PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
	PREFIX rdfs: <http://www.w3.org/2000/01/rdf-schema#>
	PREFIX xsd: <http://www.w3.org/2001/XMLSchema#>
	PREFIX owl: <http://www.w3.org/2002/07/owl#>
	PREFIX owl2xml: <http://www.w3.org/2006/12/owl2-xml#>
	PREFIX dc: <http://purl.org/dc/elements/1.1/>
	PREFIX dcterms: <http://purl.org/dc/terms/>

You can change the prefixes in the Prefixes function.



Requirements
-------------------------------------------------------------
A copy of the compiled dotNetRDF DLL and some other dependences are included.  For the latest
version and for more details visit http://dotnetrdf.org/



Usage
-------------------------------------------------------------

Perform a simple SPARQL query:

	SparqlResultSet rs = Stardog.Query("select * where { ?s ?p ?o };");

	foreach(SparqlResult r in rs)
	{
		Console.WriteLine(r["s"].ToString();
	}

Perform a SPARQL 1.1 update:
	
	Stardog.Update("delete where { :node1 dcterms:audience ?o }; insert data { :node2 dc:title \"My name\"; };");




The MIT License
-------------------------------------------------------------
Copyright (c) 2012 Ron Michael Zettlemoyer
				
Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.