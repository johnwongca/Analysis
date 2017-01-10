/*test cursor and image*/
//window.resizeTo(1000,1000);
//window.resizeTo(800, 800);
/*
var desktop = Sys.UI.TBase.Create("panel");
desktop.setParent(document.body);
desktop.setRect(0,0,20000,20000);
desktop.WriteCSS("right","0px","bottom","0px");
desktop.setLeft(0);
desktop.setTop(0);

desktop.setColor(Sys.TColor.Random());



var debug = Sys.UI.TBase.Create("panel");
debug.SetProps({Left:10,Top: 500, Width: 700, Height:300, Parent: desktop});
debug.WriteCSS("overflow","auto");
debug.getElement().id = "Debug";
debug.getElement().setAttribute("contentEditable", true);
*/
/*
Sys.UI.AllEvents.OnEvent.AttachEvent(aa)
function aa(evn)
{
Sys.Debug.Clear();
Sys.Debug.Writeln(desktop._EL.scrollWidth);
Sys.Debug.Writeln(desktop._EL.scrollHeight);
}
*/
/*
var o1 = Sys.UI.TBase.Create("panel");
o1.setParent(desktop);
o1.setLeft(18);
o1.setTop(28);
o1.setWidth(400);
o1.setHeight(400);
o1.setColor(Sys.TColor.Random());
o1.setCanFocus(true);
o1.__Resizable = function(){return true;};
o1.__Movable = function(){return true;};
o1.__GetMovingRect = function() {return {Left:5, Top:5, Width: this.getWidth()- 10, Height: 25};};
*/
/*
var aaa =function()
{
	var a1 = 10;
	this.aaa_m1 = function(){a1++; return a1};
}*/
var a = function()
{
	this.create.apply(this, arguments)
};
a.prototype.create = function()
{
	alert("aaa"+arguments.length);
};
a.ccc = function()
{
	var s = "", a = arguments;
	for(var i = 0; i< a.length; i++)
		s = s + "a["+i+"],";
	s = "new this("+s.left(s.length-1)+");";
	return eval(s);
}
//var b = a.ccc(1,2.3);
function cc()
{
	var a = arguments;
	var b = cc;
	var c = function()
	{
		alert("aa");
	}
	return c;
}
var f = cc(9,8,7);
var g = new f(1,2,3);
DoNothing();
