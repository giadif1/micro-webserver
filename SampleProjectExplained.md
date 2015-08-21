# Introduction #

the program.cs file of the WebServerTest project contains a small example of how the MicroWebServer can be used

# Details #

`webServer.Add(new RequestRoute("/", Resources.StringResources.Index, "text/html"));` ==> When the "/" url is requested from the server, it will return the resource file named "Index" with content type

`webServer.Add(new RequestRoute("/test", HttpMethods.GET, request => new HtmlResponse("Hello World !")));` ==> "/test" url will return "Hello World !" with "text/html" content type

`webServer.Add(new RequestRoute("/redirect", HttpMethods.GET, request => new RedirectResponse("/")));` ==> "/redirect" url will redirect to "/"

`webServer.Add(new RequestRoute("/api/time", HttpMethods.GET, GetTime));` ==> "/api/time" url will invoke the GetTime method (defined in program.cs)