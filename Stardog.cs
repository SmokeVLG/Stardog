using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VDS.RDF;
using VDS.RDF.Writing;
using VDS.RDF.Writing.Formatting;
using VDS.RDF.Update;
using VDS.RDF.Storage;
using VDS.RDF.Storage.Params;
using VDS.RDF.Parsing;
using VDS.RDF.Query;
using System.Text;


/// <summary>
/// Helper funtions to access Stardog RDF database
/// </summary>
/// 
public class Stardog
{

    /// <summary>
    /// Encode your commonly used prefixes here; they'll automatically be added to all queries
    /// </summary>
    /// <returns></returns>
    public static string Prefixes()
    {
        return "PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>" + System.Environment.NewLine
        + "PREFIX rdfs: <http://www.w3.org/2000/01/rdf-schema#> " + System.Environment.NewLine
        + "PREFIX xsd: <http://www.w3.org/2001/XMLSchema#>" + System.Environment.NewLine
        + "PREFIX owl: <http://www.w3.org/2002/07/owl#>" + System.Environment.NewLine
        + "PREFIX owl2xml: <http://www.w3.org/2006/12/owl2-xml#>" + System.Environment.NewLine
        + "PREFIX dc: <http://purl.org/dc/elements/1.1/>" + System.Environment.NewLine
        + "PREFIX dcterms: <http://purl.org/dc/terms/>" + System.Environment.NewLine;
    }


    public static string ConnectionString()
    {
        return "http://localhost:5820/";
    }


    public static string ConnectionDatabase()
    {
        return "main_ontology"; 
    }

    public static string ConnectionUser()
    {
        return "admin";
    }

    public static string ConnectionPassword()
    {
        return "admin";
    }


 




    /// <summary>
    /// Pass a triple of RDF/triples and then delete those records into the database;
    /// </summary>
    /// <param name="name">URI of graph; leave null for default graph</param>
    /// <param name="graph">String representing RDF/triples to insert</param>
    /// <returns></returns>
    public static bool InsertGraph(Uri name, string graph)
    {
        Graph g = new Graph();
        g.LoadFromString(graph);
        return InsertGraph(name, g);
    }


    /// <summary>
    /// Pass a graph and then insert those records into the database;
    /// </summary>
    /// <param name="name">URI of graph; leave null for default graph</param>
    /// <param name="graph">Graph object containing data to insert</param>
    /// <returns></returns>
    public static bool InsertGraph(Uri name, Graph g)
    {
        return UpdateGraph(name, g, null);

    }


    /// <summary>
    /// Delete a graph by name from the database
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static bool DeleteGraph(Uri name)
    {

        using (StardogConnector dog = new StardogConnector(ConnectionString(), ConnectionDatabase(), ConnectionUser(), ConnectionPassword()))
        {
            dog.DeleteGraph(name);
        }
        return true;
    
    }




    /// <summary>
    /// Pass a triple of RDF/triples and then delete those records from the database;
    /// </summary>
    /// <param name="name">URI of graph; leave null for default graph</param>
    /// <param name="graph">String representing RDF/triples</param>
    /// <returns></returns>
    public static bool DeleteGraph(Uri name, string graph)
    {
        Graph g = new Graph();
        g.LoadFromString(graph);
        return DeleteGraph(name, g);
    }



    /// <summary>
    /// Pass a graph and then delete those records from the database;
    /// </summary>
    /// <param name="name">URI of graph; leave null for default graph</param>
    /// <param name="graph">Graph object containing data to delete</param>
    /// <returns></returns>
    public static bool DeleteGraph(Uri name, Graph g)
    {
        return UpdateGraph(name, null, g);

    }



    /// <summary>
    /// Delete and insert the provided graphs
    /// </summary>
    /// <param name="name">URI of graph; leave null for default graph</param>
    /// <param name="insert">String representing RDF/triples to insert</param>
    /// <param name="delete">String representing RDF/triples to delete</param>
    /// <returns></returns>
    public static bool UpdateGraph(Uri name, string insert, string delete)
    {
        Graph gInsert = new Graph();
        gInsert.LoadFromString(insert);

        Graph gDelete = new Graph();
        gDelete.LoadFromString(delete);

        return UpdateGraph(name, gInsert, gDelete);

    }

    /// <summary>
    /// Delete and insert the provided graphs
    /// </summary>
    /// <param name="name">URI of graph; leave null for default graph</param>
    /// <param name="gInsert">Graph object containing data to insert</param>
    /// <param name="gDelete">Graph object containing data to delete</param>
    /// <returns></returns>
    public static bool UpdateGraph(Uri name, Graph gInsert, Graph gDelete)
    {

        using (StardogConnector dog = new StardogConnector(ConnectionString(), ConnectionDatabase(), ConnectionUser(), ConnectionPassword()))
        {
            dog.UpdateGraph(name, gInsert.Triples, gDelete.Triples);
        }
        return true;

    }


    /// <summary>
    /// Execute insert or delete SPARQL queries against Stardog;
    /// separate multiple statements with a semicolon
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public static bool Update(StringBuilder command)
    {
        return Update(command.ToString());
    }



    /// <summary>
    /// Execute insert or delete SPARQL queries against Stardog;
    /// separate multiple statements with a semicolon
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public static bool Update(string command)
    {

        using (StardogConnector dog = new StardogConnector(ConnectionString(), ConnectionDatabase(), ConnectionUser(), ConnectionPassword()))
        {

            try
            {

                dog.Begin(); // wrap all the statements in this command into one transaction; otherwise stardog will run them as separate transactions

                GenericUpdateProcessor processor = new GenericUpdateProcessor(dog);

                SparqlUpdateParser parser = new SparqlUpdateParser();

                processor.ProcessCommandSet(parser.ParseFromString(Prefixes() + command));

                dog.Commit();

                return true;

            }
            catch (Exception err)
            {

                try
                {
                    dog.Rollback();
                }
                catch { }

                throw err;

            }

        }


    }


 


    /// <summary>
    /// Exdecute a SPARQL query and return the results
    /// </summary>
    /// <param name="command">Your SPARQL query</param>
    /// <param name="rm">Reasoning mode; default is none</param>
    /// <returns></returns>
    public static SparqlResultSet Query(string command, StardogReasoningMode rm = StardogReasoningMode.None)
    {
        using (StardogConnector dog = new StardogConnector(ConnectionString(), ConnectionDatabase(), rm, ConnectionUser(), ConnectionPassword()))
        {
            Object results = dog.Query(command);
            
            if (results is SparqlResultSet)
            {
                SparqlResultSet rset = (SparqlResultSet)results;
                return rset;
            }
            else
            {
                throw new Exception("query failed " + command);

                return null;
            }
        }


    }



    /// <summary>
    /// Take a node (subject, predicate, object) and return it
    /// properly formatted as a string you could use in SPARQL;
    /// ie http://fynydd.com = <http://fynydd.com>
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string EncapNode(INode value)
    {

        switch (value.NodeType)
        {
            case NodeType.Literal:
                return "\"" + value.ToString() + "\"";
                break;
            case NodeType.Uri:
                return "<" + value.ToString() + ">";
                break;
            case NodeType.Blank:
                return "_:blank";
                break;
            case NodeType.GraphLiteral: // not sure if this is right
                return "\"" + value.ToString() + "\"";
                break;
            case NodeType.Variable: // not sure if this is right or matters 
                return "?" + value.ToString();
                break;


        }

        return "";


    }




}