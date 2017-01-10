<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="_Default" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server"><title>Base Class Design</title>   
<script id = "ScriptLoaderDoNotCreateSameTagName" type="text/javascript" language="javascript" >
function LoadJavaScript(file){
    var script = document.createElement('script'); 
    script.type = 'text/javascript'; 
    script.src = file; 
    document.getElementById("ScriptLoaderDoNotCreateSameTagName").parentNode.appendChild(script);
}



			var G_vmlCanvasManager_ =   {
											init: 
												function (opt_doc) 
												{
													var doc = opt_doc || document;
													if (/MSIE/.test(navigator.userAgent) && !window.opera) 
													{
														var self = this;
														doc.attachEvent("onreadystatechange", 
																			function () 
																			{
																				self.init_(doc);
																			}
																		);
													}
												},
											init_: 
												function (doc, e) 
												{
													if (doc.readyState == "complete") 
													{
														// create xmlns  xmlns:JB 
														/*if (!doc.namespaces["g_vml_"]) 
														{
															doc.namespaces.add("g_vml_", "urn:schemas-microsoft-com:vml");
														}
														if (!doc.namespaces["JB"]) 
														{
															doc.namespaces.add("JB");
														}*/
														// setup default css
														var ss = doc.createStyleSheet();
														ss.cssText = "zzz{display:inline-block;overflow:hidden; text-align:left;font-size: 120pt;}}";
														// find all canvas elements
														var els = doc.getElementsByTagName("zzz");
														for (var i = 0; i < els.length; i++) 
														{
															if (!els[i].getContext) 
															{
																this.initElement(els[i]);
															}
														}
													}
												},
											fixElement_: 
												function (el) 
												{
													// in IE before version 5.5 we would need to add HTML: to the tag name
													// but we do not care about IE before version 6
													var outerHTML = el.outerHTML;
													var newEl = document.createElement(outerHTML);
													// if the tag is still open IE has created the children as siblings and
													// it has also created a tag with the name "/FOO"
													if (outerHTML.slice(-2) != "/>") 
													{
														var tagName = "/" + el.tagName;
														var ns;
														// remove content
														/*while ((ns = el.nextSibling) && ns.tagName != tagName) 
														{
															ns.removeNode();
														}*/
														// remove the incorrect closing tag
														/*if (ns) 
														{
															ns.removeNode();
														}*/
													}
													el.parentNode.replaceChild(newEl, el);
													newEl.innerText = "abce";
													return newEl;
												},
				
											initElement: 
												function (el) 
												{
													el = this.fixElement_(el);
													/*el.getContext = function () 
																	{
																		if (this.context_) 
																		{
																			return this.context_;
																		}
																		return this.context_ = new CanvasRenderingContext2D_(this);
																	};
													var self = this; //bind
													el.attachEvent("onpropertychange", 
																	function (e) 
																	{
																		// we need to watch changes to width and height
																		switch (e.propertyName) 
																		{
																			case "width":
																			case "height":
																			// coord size changed?
																			break;
																		}
																	});
													// if style.height is set
													var attrs = el.attributes;
													if (attrs.width && attrs.width.specified) 
													{
														// TODO: use runtimeStyle and coordsize
														// el.getContext().setWidth_(attrs.width.nodeValue);
														el.style.width = attrs.width.nodeValue + "px";
													}
													if (attrs.height && attrs.height.specified) 
													{
														// TODO: use runtimeStyle and coordsize
														// el.getContext().setHeight_(attrs.height.nodeValue);
														el.style.height = attrs.height.nodeValue + "px";
													}
													//el.getContext().setCoordsize_()*/
												}
										};
			G_vmlCanvasManager_.init();



</script>
<STYLE TYPE="text/css">
@media all{JB\:gr {font-size: 20pt; font-style: italic; font-family: Arial;color:green; text-decoration: underline}}
@media all{JB\:bk {background-color: yellow; height:40;width:100;font-size: 40pt;}}
@media all{JB\:wide {text-decoration: line-through; color:purple;text-transform: uppercase;font-size: 60pt;}}
@media kkk{display:inline-block;overflow:hidden; text-align:left;font-size: 120pt;}
zzz{display:inline-block;overflow:hidden; text-align:left;font-size: 120pt;}}
</STYLE> 

</head>

<body scroll = "no" ><!--onload = "LoadJavaScript('Sys/Starter.js');" -->
<img src="data:image/png;base64,
iVBORw0KGgoAAAANSUhEUgAAAAoAAAAKCAYAAACNMs+9AAAABGdBTUEAALGP
C/xhBQAAAAlwSFlzAAALEwAACxMBAJqcGAAAAAd0SU1FB9YGARc5KB0XV+IA
AAAddEVYdENvbW1lbnQAQ3JlYXRlZCB3aXRoIFRoZSBHSU1Q72QlbgAAAF1J
REFUGNO9zL0NglAAxPEfdLTs4BZM4DIO4C7OwQg2JoQ9LE1exdlYvBBeZ7jq
ch9//q1uH4TLzw4d6+ErXMMcXuHWxId3KOETnnXXV6MJpcq2MLaI97CER3N0
vr4MkhoXe0rZigAAAABJRU5ErkJggg==" alt="Red dot" />
<JB:gr>This is the effect</JB:gr>
<JB:bk>This is the effect</JB:bk>
<JB:wide>This is the effect</JB:wide>
<JB:accccc>This is the effect</JB:accccc>
<script type="text/javascript">
</script>
<zzz width="400" height="300">-----afadfad-----</zzz>
<kkk>--kkk--</kkk>
<!--
<script type="text/javascript" language="javascript" src="Sys/Starter.js"> </script>
-->
</body>
</html>
 