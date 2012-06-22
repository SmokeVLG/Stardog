
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
		Console.WriteLine(r["s"].ToString();

    ... or ...

	var records = (from record in Stardog.Query("select * where { ?s ?p ?o }")
		select new {
			s = record["s"].ToString(),
			p = record["p"].ToString(),
			o = record["o"].ToString()
		});

	foreach(var record in records)
		Console.WriteLine(record.s);

Perform a SPARQL 1.1 update:
	
	Stardog.Update("delete where { :node1 dcterms:audience ?o }; insert data { :node2 dc:title \"My name\"; };");

There are other functions that you can explore and will be documented later.



New BSD License
-------------------------------------------------------------
Copyright (c) 2012, Ron Michael Zettlemoyer.
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:

- Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.

- Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the
documentation and/or other materials provided with the distribution.

- Neither the names of this software's contributors nor the names of the
contributors' organizations may be used to endorse or promote products
derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
