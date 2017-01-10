/*
    Author:         John Huang
    Start Date:     2006-04-29 
    Description:    Debug
*/
(function()
{
    var obj = Sys.Debug;
    /*The methods after here for this class is for test purpose*/
    obj.Clear = function()
    {
        var c = document.getElementById("Debug");
        if(!c) return;
        c.innerHTML = "";
    }
    obj.ShowProperties = function()
    {
        for(var j=0; j<arguments.length; j++)
        {
            this.Writeln('--------------------------------------------------');
            this.Writeln(arguments[j]);
            for(var i in arguments[j])
            {
                this.Writeln(i);
            }
        }
    }
    obj.ShowPropertiesWithValue = function()
    {
        for(var j=0; j<arguments.length; j++)
        {
            this.Writeln('--------------------------------------------------');
            this.Writeln(arguments[j]);
            for(var i in arguments[j])
            {
                try
                {
                    this.Writeln(i+" = "+arguments[j][i]+";");
                }catch(e)
                {
                    this.Writeln(i+" = Could not read Value; Error = " +e);
                }
            }
        }
    }
    obj.Write = function()
    {
        var c = document.getElementById("Debug");
        if(!c)
        {
            for(var i=0; i<arguments.length; i++ )
            {
                try
                {
                    document.write(arguments[i].toString());
                }
                catch(e)
                {
                    //document.write("Could not read value. ");
                    document.write("Could not read value. Error = "+e.toString());
                }
            }
        }
        else
        {
            for(var i=0; i<arguments.length; i++ )
            {
                try
                {
                    c.innerHTML = c.innerHTML+"<span>"+arguments[i]+"<span>";
                }
                catch(e)
                {
                    //c.innerHTML = c.innerHTML+"<span>Could not read value. <span>";
                    c.innerHTML+"<span>Could not read value. Error = "+e.toString()+"<span>";
                }
            }
        }
    }
    obj.Writeln = function()
    {
        var c = document.getElementById("Debug");
        if(!c)
        {
            for(var i=0; i<arguments.length; i++ )
            {
                try
                {
                    document.write("<div>"+arguments[i].toString()+"</div>");
                }
                catch(e)
                {
                    document.write("<div>Could not read value. Error = "+e.toString()+"<div>");
                    //document.write("<div>Could not read value.<div>");
                }
                
            }
        }
        else
        {
            for(var i=0; i<arguments.length; i++ )
            {
                try
                {
                    c.innerHTML = c.innerHTML+"<div>"+arguments[i]+"</div>";
                }
                catch(e)
                {
                    c.innerHTML = c.innerHTML+"<div>Could not read value. Error = "+e.toString()+"<div>";
                    //c.innerHTML = c.innerHTML+"<div>Could not read value.<div>";
                }
            }
        }
    }
})();