/*
    Author:             John Huang
    Start Date:         2006-04-29 
    Last Modification:  2006-05-06
    Description:        Define common classes
*/
/*Namespaces*/ 
function registerNamespace(ns)
{
	/*ns can be "Sys.Obj" */
    var root = window;
    var items= ns.split(".");
    for(var i = 0; i < items.length; i++)
    {
        var item = items[i];
        if(!root[item]) root[item] = new Object();
        root = root[item];
    }
    return eval(ns);
};
registerNamespace("Sys");
registerNamespace("Sys.Drawing");
Sys.version = "0.00";
Sys.argsToArray = function(arg)
{
    var r = new Array();
    for(var i=0; i<arg.length; i++)
        r[i] = arg[i];
    return r;
};
Sys.copyValues = function(src, desc)
{
	var t;
	for(var i in src)
	{	
		if(!desc[i])
			continue;
		t = typeof(src[i])
		if( t == "number" || t == "string" || t == "boolean")
			desc[i] = src[i];
	};
};
Sys.applyObject = function(src, desc)
{
	for(var i in src)
		if( typeof(src[i]) != "function")
			desc[i] = src[i];
};

/*Type*/
(function()
{
	window.Type = Function;
	Sys.Type = Type;
	var obj = Type;
})();
/*Function*/
(function()
{
	var obj = Function;
	obj._typeName = "Function";
	obj.abstractMethod=function()
	{
		throw new Sys.Error(1000); //"Could not call an abstract method directly, it must be implemented first";
	};
	obj.registerNamespace = registerNamespace;
	obj.createCallback = function(callBackFunction, sender) 
	{
		return function(){return callBackFunction(sender);};
	};
	obj.createDelegate = function(caller, callBackFunction)
	{
		return	function()
				{
					if(arguments.length == 0)
						return callBackFunction.apply(caller);
					return callBackFunction.apply(caller, arguments);
				};
	};
	obj.createAsyncDelegate = function(caller, callBackFunction, delay)
	{
		if(!delay) delay = 1;
		return  function()
				{
					var _a = arguments;
					function fnc()
					{
						if(_a.length == 0)
							callBackFunction.apply(caller);
						else
							callBackFunction.apply(caller, _a);
					}
					window.setTimeout(fnc, delay);
					return true;
				};
	};
	obj.createInterface=function(interfaceName)
	{
		var ret = function(){}
		ret._typeName=interfaceName;
		ret._interface=true;
		ret._abstract=true;
		ret._sealed=true;
		eval(interfaceName + " = ret");
		return ret;
	};
	obj.createEnum = function(enumName)
	{
		var enumeration={};
		if(enumName)
			eval("enumeration="+enumName+"={};");
		enumeration.getValues = function()
		{
			if(!enumeration._values)
			{
				var values={};
				for(var f in enumeration)
				{
					if(typeof(enumeration[f]) != "function")
						values[f]=enumeration[f];
				}
				enumeration._values = values;
			}
			return enumeration._values;
		};
		enumeration.extend=function(enumValue)
		{
			if(enumValue)
			{
				for(var i in enumValue.getValues())
				{
					enumeration[i]=i;
				}
				enumeration._values=null;
			}
			return enumeration;
		};
		enumeration.parse = function(str)
		{
			if(str)
			{
				for(var f in enumeration)
				{
					if(f.toLowerCase()===str.toLowerCase() && typeof(enumeration[f])!="function")
						return enumeration[f];
				}
			}
			return null;
		};
		enumeration.toString=function(value)
		{
			for(var i in enumeration)
			{
				if(enumeration[i]===value)
					return i;
			}
			throw new Sys.Error(1005);
		};
		enumeration.getName=function()
		{
			return enumName;
		};
		enumeration.isEnum=function()
		{
			return true;
		};
		for(var i=1;i<arguments.length;i+=2)
		{
			enumeration[arguments[i]]=arguments[i+1];
		}
		return enumeration;
	};
	obj.createSimpleAnonymousEnum = function()
	{
		var arrEnum=[];
		arrEnum.push(null);
		for(var i=0; i<arguments.length; i++)
		{
			arrEnum.push(arguments[i]);
			arrEnum.push(arguments[i]);
		}
		return Type.createEnum.apply(this,arrEnum);
	};
	obj.createFlags=function(flagName)
	{	
		var flags={};
		if(flagName)
			eval("flags="+flagName+"={};");
		flags.parse=function(str)
		{
//			1. str can be a|b|c ....
			var parts=str.split("|");
			var value=0;
			for(var i = parts.length-1; i>=0; i--)
			{
				var part = parts[i].trim();
				var found=false;
				for(var f in flags)
				{
					if(f == part)
					{
						value |= flags[f];
						found=true;
						break;
					}
				}
				if(found==false)
					throw new Sys.Error(1000);
			}
			return value;
		};
		flags.toString=function(value)
		{
			var sb = Sys.StringBuilder.createInstance();
			for(var i in flags)
			{
				if((flags[i]&value)!=0)
				{
					if(sb.isEmpty() == false)
						sb.append(" | ");
					sb.append(i);
				}
			}
			return sb.toString();
		};
		flags.getName=function()
		{
			return flagName;
		};
		flags.isFlags=function()
		{
			return true;
		};
		for(var i=1; i<arguments.length; i += 2)
			flags[arguments[i]]=arguments[i+1];
		return flags;
	};
	obj.createSimpleAnonymousFlags = function()
	{
		var arrFlag = [];
		arrFlag.push(null);
		for(var i=0; i<arguments.length; i++)
		{
			arrFlag.push(arguments[i]);
			arrFlag.push(Math.pow(2, i));
		}
		return Type.createFlags.apply(this, arrFlag);
	};
	obj.emptyFunction = obj.emptyMethod = function(){};
	obj.parse = function(str)
	{
		// it is use to find a class or a function
		if(!Function._htClasses)
			Function._htClasses = {};
		var fn = Function._htClasses[str];
		if(!fn)
		{
			try
			{
				eval("fn = " + str);
				if(typeof(fn) != "function")
					fn = null;
				else 
					Function._htClasses[str] = fn;
			}
			catch(ex)
			{
			}
		}
		return fn;
	};
	obj.instanceOf = function(objectRef, classRef)
	{
		var c = typeof classRef == "function" ? classRef : Type.parse(classRef);
		if(!c)
			return false;
		if(typeof(objectRef) == "undefined" || objectRef == null)
			return false;
		if(objectRef instanceof c)
			return true;
		var instanceType = Object.getType(objectRef);
		if(instanceType == c)
			return true;
		if(!instanceType.inheritFrom)
			return false;
		return instanceType.inheritFrom(c);
	};
	var obj = Function.prototype;
	obj.getName = function()
	{
		return this._typeName;
	};
	obj.getBaseType = function()
	{
		if (this._bases)
			return this._bases[0];
		return null;
	};
	obj.createClass = function(className)
	{
		var _copyProps = function(src, desc)
		{
			for(var propName in src.prototype)
			{
				var ref = src.prototype[propName];
				if(!desc.prototype[propName])
					desc.prototype[propName] = ref;
			}
		};
		var _setClassLink = function(_parent, _child)
		{
			if(!_parent._children)
				_parent._children = [];
			if(!_child._bases)
				_child._bases = _child._parents = [];
			_parent._children.push(_child);
			_child._bases.push(_parent);	
		};
		var _setInterfaceLink = function(_parent, _child)
		{
			if(!_parent._children)
				_parent._children = [];
			if(!_child._interfaces)
				_child._interfaces = [];
			_parent._children.push(_child);
			_child._interfaces.push(_parent);
		};
		if (this._sealed)
			throw new Sys.Error(1007);  //"Could not inherit from a sealed class.";
		if(this._interface)
			throw new Sys.Error(1008);  //"Could not inherit from an interface.";
		var ret = function()
		{
			var privateData = {}; 
			this._ = function(){return privateData;};
			if(typeof(this.create) == "function")
			{
				if(arguments.length == 0)
					this.create.apply(this);
				else
					this.create.apply(this, arguments);
			}
		};
		ret._typeName = className;
		eval(className + " = ret;");
		_setClassLink(this, ret);
		//set _interfaces and _bases
		for(var i = 1; i < arguments.length; i++)
		{
			var ref = arguments[i];
			ref = typeof(ref) == "function" ? ref : Type.parse(ref);
			if(!ref)
				continue;
			if(ref._interface)
				_setInterfaceLink(ref, ret);
			else if(!ref._sealed)
				_setClassLink(ref, ret);
		}
		if(ret._bases)
		{
			for(var i = 0; i < ret._bases.length; i++)
				_copyProps (ret._bases[i], ret);
		}
		if(ret._interfaces)
		{
			for(var i = 0; i < ret._interfaces.length; i++)
				_copyProps (ret._interfaces[i], ret);
		}
		if(!ret.prototype.create)
			ret.prototype.create = Type.emptyMethod;
		if(!ret.prototype.instanceOf)
			ret.prototype.instanceOf = function(classRef)
			{
				return Type.instanceOf(this, classRef)
			}
		return ret;
	};
	obj.createAbstractClass = function(className)
	{
		var ret = this.createClass.apply(this, arguments)
		ret._abstract = true;
		return ret;
	};
	obj.createSealedClass = function(className)
	{
		var ret = this.createClass.apply(this, arguments)
		ret._sealed = true;
		return ret;
	};
	obj.inheritFrom = obj.implementInterface = function(classRef)
	{
		if(classRef==this)
			return true;
		if(this._bases)
		{
			for(var i=0; i < this._bases.length;i++)
			{
				if(this._bases[i].inheritFrom(classRef))
					return true;
			}
		}
		if(this._interfaces)
		{
			for(var i=0; i < this._interfaces.length;i++)
			{
				if(this._interfaces[i].inheritFrom(classRef))
					return true;
			}
		}
		return false;
	};
	obj.create = createInstance = function()
	{
		//var ret = new this();
		//ret.create.apply(ret, arguments);
		//return ret;
		if(arguments.length==0)
			return new this();
		var s = "", a = arguments;
		for(var i = 0; i< a.length; i++)
			s = s + "a["+i+"],";
		s = "new this("+s.left(s.length-1)+");";
		return eval(s);
	};
})();
/*Object*/
(function()
{
	var obj = Object;
	obj._typeName="Object";
	obj.getType=function(objectRef)
	{
		var c = objectRef.constructor;
		if(!c || typeof(c)!="function" || !c._typeName)
			return Object;
		return objectRef.constructor;
	};
	obj.getTypeName=function(ObjectRef)
	{
		return Object.getType(ObjectRef).getName();
	};
//	obj.fromJSON=function(O)
//	{
//		try
//		{
//			return !/[^,:{}\[\]0-9.\-+Eaeflnr-u \n\r\t]/.test(O.replace(/"(\\.|[^"\\])*"/g,""))&&eval("("+O+")");
//		}
//		catch(e)
//		{
//			return false;
//		}
//	};
	obj = obj.prototype;
})();
/*Boolean*/
(function()
{
	var obj = Boolean;
	obj._typeName = "Boolean";
	obj.parse = function(value)
	{
		if(typeof(value) == "string")
			return value.trim().toLowerCase() == "true";
		return value ? true : false;
	};
	obj.format = function(fmtStr, value)
	{
//		1. value true ; value false
		var r = fmtStr.indexOf(";");
		if(r == -1) return fmtStr;
		return value ? fmtStr.substring(0, r) : fmtStr.substring(r+1, fmtStr.length);
	};
	obj.toInteger = function(value)
	{
		return value == true ? 1 : 0;
	};
	obj = obj.prototype;
	obj.format = function(fmtStr)
    {
        return Boolean.format.call(this, fmtStr, this);
    };
    obj.toInteger = function()
    {
        return Boolean.toInteger(this);
    };
})();
/*Number*/
(function()
{
	var obj = Number;
	obj._typeName = "Number";
	obj.parse=function(str)
	{
//		str: a string value required, 
//		fractionDigits: optional, integer
		if(!str||str.length == 0)
			return 0;	
		return parseFloat(str);
	};
	obj.parseInt = function(str, radix)
	{
		return parseInt(parseFloat(str), radix);
	};
	obj.isNumber = function(value)
	{
		return !(isNaN(Number.parse(value)));
	}
	obj.isInteger = function(str, radix)
	{
		return !(isNaN(parseInt(str, radix)));
	};
	obj.format = function(fmtStr, value)
	{
//		fmtStr:
//				1. positive format;negative format
//				2. if format string contains ",", it means formated value contain thousand separators
//				3. 0.00e+0
//				4. Number.format("0-000-000-0000",12504831891) 1-250-483-1891
		if(arguments.length == 1)
			return arguments[0].toString();
		if(!fmtStr)
			return value.toString();
		var r;
		r = fmtStr.indexOf(";");
		var fmt = r >= 0?(value >= 0 ? fmtStr.substring(0,r) :  fmtStr.substring(r +1)):(value >= 0 ? fmtStr : "-" + fmtStr);
		if(!fmt)
			return value.toString();
		var t = fmt.indexOf(",") >= 0; //thousand separator
		if(t)
			fmt = fmt.replace(/,/g, "");        
		var value = Math.abs(value), numI="", numF="", numE="", firstP = -1, posE= -1, posD = -1, i, j, k, c, e = 0;
		value = value.toString();
		numI = value.match(/\d+\.{0,1}/)[0].match(/\d/g);
		if(!numI) numI=new Array();
		numF = value.match(/\.\d+/);
		if(numF)
			numF = numF[0].match(/\d/g);
		if(!numF) numF=new Array();
		fmt = fmt.match(/r\d{1,2}|e0+|e\+0+|e-0+|\.|./gi);
		r = /(r\d{1,2})/i;
		var r1 = /e0+|e\+0+|e-0+/i;
		for(i=0;i<fmt.length;i++)
		{
			if(fmt[i]=="."&&posD == -1)
				posD = i;
			if(posE== -1&&r1.test(fmt[i]))
				posE = i;
			if(r.test(fmt[i]))
				fmt[i] = fmt[i].replace(r, value.toString(RegExp.$1.replace(/r/gi,"").ToInteger()));
		}
		r = /e0+|e\+0+|e-0+/i;
		j = posD==-1 ? fmt.length:posD;
		if(value!=0&&posE>=0)
		{
			c = 0; e = 0;
			for(i=0;i<j;i++)
				if(fmt[i]=="0"||fmt[i]=="#") c++;
			while(true)
			{
				if(numI.length==1 &&numI[0]=="0")
					numI.pop();
				else 
				{
					if(numI.length == c)
						break;
					else if(numI.length > c)
					{
						numF.unshift(numI.pop());
						e++;
					}
					else if(numI.length < c)
					{
						if(numF.length==0)
							numI.push(0);
						else
							numI.push(numF.shift());
						e--;
					}
					else
						break;
				}
			}
			/*fmtStr Exponent*/
			r = /e0+/i
			if(r.test(fmt[posE]))
				fmt[posE] = fmt[posE].substring(0, 1)+e.format(fmt[posE].substring(1, fmt[posE].length));
			else
			{
				r = fmt[posE].substring(2, fmt[posE].length);
				r="+"+r+";-"+r;
				fmt[posE] = fmt[posE].substring(0, 1)+e.format(r);
			}
		}
		k=-1;
		for(i=0; i<j; i++)
		{
			if(fmt[i]=="0"||fmt[i]=="#")
			{
				if(firstP == -1)
					firstP = i;
				if(k==-1 && fmt[i]=="0")
					k++;
				else if(k==0&&fmt[i]=="#")
					fmt[i] = "0";
			}
		}
		if(firstP==-1)
			firstP=posD;
		c=0;k=-1;
		for(i=j; i<fmt.length; i++)
			if(fmt[i]=="0"||fmt[i]=="#") c++;
		if(c < numF.length)
		{
			if(numF[c]>="5")
			{
				value =parseFloat(numI.join("")+"."+numF.join("")).toFixed(c);
				value = value.toString();
				numI = value.match(/\d+\.{0,1}/)[0].match(/\d/g);
				if(!numI) numI=new Array();
				numF = value.match(/\.\d+/);
				if(numF)
					numF = numF[0].match(/\d/g);
				if(!numF) numF=new Array();
			}
		}
		c = 0;
		for(i=j;i>=0;i--)
		{
			if(fmt[i]=="0"||fmt[i]=="#")
			{
				k=-10;
				if(numI.length>0)
					fmt[i]=numI.pop();
				else
					fmt[i]= fmt[i]=="#"? "":fmt[i];
				c++;
				if(c==3&&t)
				{
					c = 0; k = i;
					fmt.splice(i,0,",");
					posD = posD==-1 ? posD : posD+1;
				}
			}
		}
		if(k != -10)
			fmt.splice(k,1);
		if(numI.length>0&&firstP >= 0)
		{
			while(numI.length>0)
			{
				k = -10;
				fmt.splice(firstP,0,numI.pop());
				posD = posD==-1 ? posD : posD+1;
				c++;
				if(c==3&&t)
				{
					c = 0; k = firstP;
					fmt.splice(firstP,0,",");
					posD = posD==-1 ? posD : posD+1;
				}
			}
		}
		if(k != -10)
			fmt.splice(k,1);
		var lastP = -1;
		for(i=posD; i<fmt.length; i++)
		{
			if(i==-1) break;
			if(fmt[i]=="0"||fmt[i]=="#")
			{
				lastP = i;
				if(numF.length>0)
					fmt[i] = numF.shift();
				else
					fmt[i] = "0";
			}
		}       
		return fmt.join("");
	};
	obj = obj.prototype;
	obj.format = function(fmtStr)
	{
		return Number.format(fmtStr, this);
	};
	obj.round = function(fractions)
	{
		return Math.round2(this, fractions);
	};
	obj.truncate = function(fractions)
	{
		return Math.trancate(this, fractions);
	};
})();
/*Date*/
(function()
{
	var obj = Date;
	obj._typeNmae = "Date";
	obj.currentFormat = function()
	{
		return Date.format[Date.format.current];
	};
	obj.daysInAMonth = function(year, month)  
	{
//		month: 0..11
		if(month==3||month==5||month==8||month==10)
			return 30;
		if(month==1)
			return Date.isLeapYear(year) == true? 29:28;
		return 31;
	};
	obj.daysInAYear = function(year)
	{
		return Date.isLeapYear(year)==true? 366:365;
	};
	obj.isLeapYear = function(year)
	{
		return (year % 4 == 0) && ((year % 100 != 0) || (year % 400 == 0));
	};
	obj.parse = function(fmtStr, str)
	{
		var style = Date.currentFormat();
		var year = 0, month = 0, day = 0, hour = 0, minute = 0, second = 0, millisecond = 0, a = -1;
		if(arguments.length == 1)
		{
			str = arguments[0];
			fmtStr = style["default"];
		}
		var _datetimeFormatToArray = function(fmt)
		{
			var ret =[], i = -1, j, k;
			var styles = Date.format.formats;
			var _SortFunc = function(a,b)
			{
				if(a.index > b.index) return 1;
				if(a.index < b.index) return -1;
				if(a.index == b.index) return 0;
			};
			fmt = fmt.toLowerCase();
			for(j in styles)
			{
				for(k in styles[j])
				{
					i = fmt.indexOf(styles[j][k]);
					if(i > -1)
					{
						ret.push({name: j, index : i, length : styles[j][k].length, fmtStr : styles[j][k]});
						break;
					}
				}
			}
			ret.sort(_SortFunc);
			return ret;
		};
		var aFormat = _datetimeFormatToArray(fmtStr);
		var _arrayIndexOf = function(a, index)
		{
			for(var z in a)
			{
				if(typeof(a[z]) == "string")
				{
					if(a[z].toLowerCase() == str.substr(index, a[z].length).toLowerCase())
						return z;
				}
			}
			return -1;
		};
		var f_pre_pos = 0, f_pre_len = 0, f_gap = 0, obj = null;
		var r_pre_pos = 0, r_cur_pos = 0, r_pre_len = 0;
		for(var idx in aFormat)
		{
			obj = aFormat[idx];
			if(typeof(obj)=="function")
				continue;
			f_gap = obj.index - f_pre_len - f_pre_pos;                
			r_cur_pos = r_pre_pos + r_pre_len + f_gap;
			switch(obj.name) 
			{
				case "year":
					switch(obj.fmtStr)
					{
						case "yy":
							year = Number.parseInt("20"+str.substr(r_cur_pos, 2));
							r_pre_pos = r_cur_pos; r_pre_len = 2;
							break;
						case "yyyy":
							year = Number.parseInt(str.substr(r_cur_pos, 4));
							r_pre_pos = r_cur_pos; r_pre_len = 4;
					}
					break;
				case "month":
					switch(obj.fmtStr)
					{
						case "m":
							if(Number.isInteger(str.substr(r_cur_pos,1))&&Number.isInteger(str.substr(r_cur_pos+1,1)))
							{
								month = Number.parseInt(str.substr(r_cur_pos,2));
								if(month <= 12)
								{
									r_pre_pos = r_cur_pos; r_pre_len = 2; month --;
									break;
								}
							}
							month = Number.parseInt(str.substr(r_cur_pos,1));
							r_pre_pos = r_cur_pos; r_pre_len = 1; month --;
							break;
						case "mm":
							month = Number.parseInt(str.substr(r_cur_pos,2));
							r_pre_pos = r_cur_pos; r_pre_len = 2; month --;
							break;
						case "mmm":
							month = _arrayIndexOf.call(str, style.shortMonths,r_cur_pos);
							if(month < 0)
								throw new Sys.Error(1001);//"Could not convert Month";
							r_pre_pos = r_cur_pos; r_pre_len = style.shortMonths[month].length;
							break;
						case "mmmm":
							month = _arrayIndexOf.call(str, style.longMonths,r_cur_pos);
							if(month < 0)
								throw new Sys.Error(1001);//"Could not convert Month";
							r_pre_pos = r_cur_pos; r_pre_len = style.longMonths[month].length;
							break;
					}
					break;
				case "weekDay":
					switch(obj.fmtStr)
					{
						case "wk":
							var t = _arrayIndexOf.call(str,style.shortWeekdays,r_cur_pos);
							if(t < 0)
								throw new Sys.Error(1002) //"Could not convert week day";
							r_pre_pos = r_cur_pos; r_pre_len = style.shortMonths[t].length;
							break;
						case "wkl":
							var t = _arrayIndexOf.call(str, style.longWeekdays,r_cur_pos);
							if(t < 0)
								throw new Sys.Error(1002) //"Could not convert week day";
							r_pre_pos = r_cur_pos; r_pre_len = style.longWeekdays[t].length;
							break;
					}
					break;
				case "day":
					switch(obj.fmtStr)
					{
						case "d":
							if(Number.isInteger(str.substr(r_cur_pos,1)) && Number.isInteger(str.substr(r_cur_pos+1,1)))
							{
								day = Number.parseInt(str.substr(r_cur_pos,2));
								r_pre_pos = r_cur_pos; r_pre_len = 2;
								break;
							}
							day = Number.parseInt(str.substr(r_cur_pos,1));
							r_pre_pos = r_cur_pos; r_pre_len = 1; 
							break;
						case "dd":
							day = Number.parseInt(str.substr(r_cur_pos,2));
							r_pre_pos = r_cur_pos; r_pre_len = 2;
							break;
					}
					break;
				case "hour":
					switch(obj.fmtStr)
					{
						case "h":
							if(Number.isInteger(str.substr(r_cur_pos,1))&&Number.isInteger(str.substr(r_cur_pos+1,1)))
							{
								hour = Number.parseInt(str.substr(r_cur_pos,2));
								r_pre_pos = r_cur_pos; r_pre_len = 2;
								break;
							}
							hour = Number.parseInt(str.substr(r_cur_pos,1));
							r_pre_pos = r_cur_pos; r_pre_len = 1; 
							break;
						case "hh":
							hour = Number.parseInt(str.substr(r_cur_pos,2));
							r_pre_pos = r_cur_pos; r_pre_len = 2;
							break;
					}
					break;
				case "minute":
					switch(obj.fmtStr)
					{
						case "n":
							if(Number.isInteger(str.substr(r_cur_pos,1))&&Number.isInteger(str.substr(r_cur_pos+1,1)))
							{
								minute = Number.parseInt(str.substr(r_cur_pos,2));
								r_pre_pos = r_cur_pos; r_pre_len = 2;
								break;
							}
							hour = Number.parseInt(str.substr(r_cur_pos,1));
							r_pre_pos = r_cur_pos; r_pre_len = 1; 
							break;
						case "nn":
							minute = Number.parseInt(str.substr(r_cur_pos,2));
							r_pre_pos = r_cur_pos; r_pre_len = 2;
							break;
					}
					break;
				case "second":
					switch(obj.fmtStr)
					{
						case "s":
							if(Number.isInteger(str.substr(r_cur_pos,1))&&Number.isInteger(str.substr(r_cur_pos+1,1)))
							{
								second = Number.parseInt(str.substr(r_cur_pos,2));
								r_pre_pos = r_cur_pos; r_pre_len = 2;
								break;
							}
							second = Number.parseInt(str.substr(r_cur_pos,1));
							r_pre_pos = r_cur_pos; r_pre_len = 1; 
							break;
						case "ss":
							second = Number.parseInt(str.substr(r_cur_pos,2));
							r_pre_pos = r_cur_pos; r_pre_len = 2;
							break;
					}
					break;
				case "millisecond":
					switch(obj.fmtStr)
					{
						case "ms":
							if(Number.isInteger(str.substr(r_cur_pos,1))&&Number.isInteger(str.substr(r_cur_pos+1,1))&&Number.isInteger(str.substr(r_cur_pos+2,1)))
							{
								millisecond = Number.parseInt(str.substr(r_cur_pos,3));
								r_pre_pos = r_cur_pos; r_pre_len = 3;
								break;
							}
							if(Number.isInteger(str.substr(r_cur_pos,1))&&Number.isInteger(str.substr(r_cur_pos+1,1)))
							{
								millisecond = Number.parseInt(str.substr(r_cur_pos,2));
								r_pre_pos = r_cur_pos; r_pre_len = 2;
								break;
							}
							millisecond = Number.parseInt(str.substr(r_cur_pos,1));
							r_pre_pos = r_cur_pos; r_pre_len = 1; 
							break;
						case "ms1":
							millisecond = Number.parseInt(str.substr(r_cur_pos,2));
							r_pre_pos = r_cur_pos; r_pre_len = 3;
							break;
					}
					break;
				case "A":
					switch(obj.fmtStr)
					{
						case "am/pm":
							var a = _arrayIndexOf.call(str, style.AM_PM,r_cur_pos);
							if(a < 0)
								throw new Sys.Error(1003); //"Could not convert AM/PM";
							r_pre_pos = r_cur_pos; r_pre_len = style.AM_PM[a].length;
							break;
						case "a/p":
							var t = _arrayIndexOf.call(str, style.A_P,r_cur_pos);
							if(t < 0)
								throw new Sys.Error(1003); //"Could not convert AM/PM";
							r_pre_pos = r_cur_pos; r_pre_len = style.A_P[a].length;
							break;
					}
					break;
			}
			f_pre_len = obj.length; 
			f_pre_pos = obj.index
		}
		if(a == 0 && hour == 12) hour = 0;
		if(a == 1 && hour < 12) hour = hour + 12;
		return new Date(year, month, day, hour, minute, second, millisecond);
	};
	obj.format = function(fmtStr, value)
	{
		value = new Date(value.valueOf());
		var r = Date.format.regularExpression, i, j, k;
		var h = value.getHours();
		var style = Date.currentFormat();
		r = fmtStr.match(r);
		if(!r) return fmtStr;
		var is12 = Date.format.regularExpressionIs12.test(fmtStr);
		for(var i = 0; i <r.length; i ++)
		{
			switch(r[i].toLowerCase())
			{
				case "yyyy":
					r[i] = value.getFullYear().toString();
					break;
				case "yy":
					r[i] = value.getFullYear().toString().right(2);
					break;
				case "mmmm":
					r[i] = style.longMonths[value.getMonth()];
					break;
				case "mmm":
					r[i] = style.shortMonths[value.getMonth()];
					break;
				case "mm":
					r[i] = ("0"+(value.getMonth()+1).toString()).right(2);
					break;
				case "m":
					r[i] = (value.getMonth()+1).toString();
					break;
				case "wkl":
					r[i] = style.longWeekdays[value.getDay()];
					break;
				case "wk":
					r[i] = style.shortWeekdays[value.getDay()];
					break;
				case "dd":
					r[i] = ("0"+value.getDate().toString()).right(2);
					break;
				case "d":
					r[i] = value.getDate().toString();
					break;
				case "hh":
					if(is12)
					{
						if(h>12)
							r[i] = ("0"+(h-12).toString()).right(2);
						else if(h >0 && h<= 12)
							r[i] = ("0"+h.toString()).right(2);
						else 
							r[i] = "00";
					}
					else
						r[i] = ("0"+h.toString()).right(2);
					break;
				case "h":
					if(is12)
					{
						if(h>12)
							r[i] = (h-12).toString();
						else if(h >0 && h<= 12)
							r[i] = h.toString();
						else 
							r[i] = "0";
					}
					else
						r[i] = h.toString();
					break;
				case "nn":
					r[i] = ("0"+value.getMinutes().toString()).right(2);
					break;
				case "n":
					r[i] = value.getMinutes().toString();
					break;
				case "ss":
					r[i] = ("0"+value.getSeconds().toString()).right(2);
					break;
				case "s":
					r[i] = value.getSeconds().toString();
					break;
				case "ms1":
					r[i] = ("00"+(value.getMilliseconds()).toString()).right(3);
					break;
				case "ms":
					r[i] = value.getMilliseconds().toString();
					break;
				case "am/pm":
					r[i] = is12?( h<12? style.AM_PM[0]:style.AM_PM[1]):"";
					break;
				case "a/p":
					r[i] = is12?( h<12? style.A_P[0]:style.AM_PM[1]):"";
			}
		}
		return r.join("");
	};
	Sys.applyObject(Sys.Resources.datetimeFormat, obj.format);
	obj = obj.prototype;
	obj.format = function(fmtStr)
	{
		return Date.format(fmtStr, this);
	}
	obj.isLeapYear = function()
    {
        return Date.isLeapYear(this.getFullYear());
	};
	obj.dateOf = function()
	{
        return new Date(this.getFullYear(), this.getMonth(), this.getDate());
	};
	obj.timeOf = function()
	{
        return new Date(0, 0, 0, this.getHours(),this.getMinutes(), this.getSeconds(), this.getMilliseconds());
	}; 
	obj.addMilliseconds = function(value)
	{
	    return new Date(this.valueOf() + value);
	};
	obj.addSeconds = function(value)
	{
	    return new Date(this.valueOf() + value*1000);
	};
	obj.addMinutes = function(value)
	{
	    return new Date(this.valueOf() + value*60000);
	};
	obj.addHours = function(value)
	{
	    return new Date(this.valueOf() + value*3600000);
	};
	obj.addDays = function(value)
	{
	    return new Date(this.valueOf() + 86400000 * value);
	};
	obj.addWeeks = function(value)
	{
	    return new Date(this.valueOf() + 604800000*value);
	};
	obj.addMonths = function(value)
	{
	    var d0 = this.getDate();
	    var date = new Date(this.getFullYear(), this.getMonth() + value,1);
	    var d1 = Date.daysInAMonth(date.getFullYear(), date.getMonth())
        date.setDate(d0<=d1 ? d0 : d1);
        date.setHours(this.getHours(), this.getMinutes(), this.getSeconds(), this.getMilliseconds());
        return date;
	};
	obj.addYears = function(Value)
	{
	    return this.addMonths(Value * 12);
	};
	obj.diffMonth = function(to)
	{
	    return to.getFullYear()*12 + to.getMonth()- this.getFullYear()*12 - this.getMonth();
	};
	obj.diffDay = function(to)
	{
	    return Math.div((to.valueOf() - this.valueOf()),86400000);
	};
	obj.diffYear = function(to)
	{
	    return to.getFullYear() - this.getMonth();
	};
	obj.diffWeek = function(to)
	{
	    return Math.div((to.valueOf() - this.valueOf()),604800000);
	};
	obj.diffMillisecond = function(to)
	{
	    return to.valueOf() - this.valueOf();
	};
	obj.diffSecond = function(to)
	{
	    return Math.div((to.valueOf() - this.valueOf()),1000);
	};
	obj.diffMinute = function(to)
	{
	    return Math.div((to.valueOf() - this.valueOf()),60000);
	};
	obj.diffHour = function(to)
	{
	    return Math.div((to.valueOf() - this.valueOf()),3600000);
	};
	obj.diff = function(type, to) //type can be year/y, month/m, day/d, week/w, hour/h, minite/n, second/s, millisecond/ms
	{
	    type = (new String(type)).toLowerCase();
	    to = new Date(to.valueOf());
	    switch(type)
	    {
	        case "m":
	        case "month":
	            return this.diffMonth(to);
	        case "d":
	        case "day":
	            return this.diffDay(to);
	        case "y":
	        case "year":
	            return this.diffYear(to);
            case "w":
	        case "week":
	            return this.diffWeek(to);
	        case "ms":
	        case "millisecond":
	            return this.diffMillisecond(to);
	        case "s":
	        case "second":
	            return this.diffSecond(to);
	        case "n":
	        case "minute":
	            return diffMinute(to);
	        case "h":
	        case "hour":
	            return diffHour(to);
	    }
	}
})();
/*string*/
(function()
{
	var obj = String;
	obj._typeName = "String";
	obj.format = function(fmtStr)
	{
//		fmtStr:	{{ -->{
//				}} -->}
//				{0}, {0:N}, {0:Nformat}
		if(!arguments) return "";
		if(arguments.length==0) return "";
		var r = arguments[0].match(/\}\}|\{\{|\{\d+\}|\{\d+:[n,b,s,d][^\}]+\}|./gi);
		if(!r)
			return fmtStr;
		var r1;
		for(var i =0; i<r.length; i++)
		{
			if(r[i].length==1) continue;
			if(r[i]=="{{") 
			{
				r[i]="{";
				continue;
			}
			if(r[i]=="}}")
			{
				r[i] = "}";
				continue;
			}
			if((/^\{\d+\}$/).test(r[i]))
			{
				r[i] = arguments[(Number.parseInt(r[i].toString().replace(/\{|\}/g,"")) + 1)];
				continue;
			}
			if((/^\{\d+:[n,b,s,d].+\}$/gi).test(r[i]))
			{
				r1 = r[i].match(/^\{\d+:|\}$|./g);
				r1[0] = Number.parseInt(r1[0].Right(r1[0].length-1));
				r1[1] = r1[1].right(1).toUpperCase();
				var fmt = r1.slice(2,-1);
				if(fmt)
					fmt = fmt.join("");
				else
				{
					fmt = "";
					r[i] = "";
					continue;
				}
				switch(r1[1])
				{
					case "N":
						r[i] = (new Number(arguments[r1[0]+1])).format(fmt);
						break;
					case "B":
						r[i] = (new Boolean(arguments[r1[0]+1])).format(fmt);
						break;
					case "S":
						r[i] = arguments[r1[0]+1].toString();
						break;
					case "D":
						r[i] = (new Date(arguments[r1[0]+1].valueOf())).format(fmt);
				}
			}
		}
		return r.join("");
	};
	obj = obj.prototype;
	obj.endWith = function(value, ignoreCase)
	{
		if(typeof(value)=="string")
			value = [value];
		for(var i in value)
		{
			if(typeof(value[i])=="string")
			{
				if(!ignoreCase)
				{
					if(this.right(value[i].length) == value[i]) 
						return true;
				}
				else
				{
					if(this.right(value[i].length).toLowerCase() == value[i].toLowerCase())
					{
						return true;
					}
				}
			}
		}
		return false;
	};
	obj.startWith = function(value, ignoreCase)
	{
		if(typeof(value)=="string")
			value = [value];
		for(var i in value)
		{
			if(typeof(value[i])=="string")
			{
				if(!ignoreCase)
				{
					if(this.left(value[i].length) == value[i]) 
						return true;
				}
				else
				{
					if(this.left(value[i].length).toLowerCase() == value[i].toLowerCase())
					{
						return true;
					}
				}
			}
		}
		return false;
	};
	obj.exists = function(value, ignoreCase)
	{
		if(typeof(value)=="string")
			value = [value];
		var str = ignoreCase? this.toLowerCase() : this;
		for(var i in value)
		{
			if(typeof(value[i])=="string")
			{
				if(!ignoreCase)
				{
					if(str.indexOf(value[i]) > -1) 
						return true;
				}
				else
				{
					if(str.indexOf(value[i].toLowerCase()) > -1) 
					{
						return true;
					}
				}
			}
		}
		return false;
	};
	obj.leftTrim = obj.lTrim = obj.trimLeft = function()
    {
        return this.replace(/\s+$/g,"");
    };
    obj.rightTrim = obj.rtrim = obj.trimRight = function()
    {
        return this.replace(/^\s+/g,"");
    };
    obj.trim = function(direction)
    {
		if(direction==1)
			return this.trimLeft();
		if(direction==-1)
			return this.trimRight();
        return this.replace(/^\s+|\s+$/g,"");
    };
    obj.left = function(len)
    {
        return this.substring(0,len);
    };
    obj.right = function(len)
    {
        return this.substring(this.length - len, this.length );
    };
    obj.replicate = obj.repeat = function(n, separator)
    {
        var ret = new Array();
        if(!separator) separator = "";
        for(var i = 0; i < n; i++)
            ret[i] = this;
        return ret.join(separator);
    };
    obj.copy = function(position, count)
    {
        if(!position)
            position = 0;
        if(!count)
            count = 1;
        return this.substr(position, count);
    };
    obj.pad = function(length, str, direction)
    {
		if(!length) return "";
		if(length <= 0) return "";
		var ret = this;
		if(!str) 
			str = "0";
		if(!direction) 
			direction = 1;
		while(ret.length < length) 
		{
			if(direction > 0) 
				ret = str + ret;
			else
				ret = ret + str;
		}
		if(ret.length > length)
		{
			if(direction > 0) 
				ret = ret.right(length);
			else
				ret = ret.left(length);
		}
		return ret;	
	};
	obj.padLeft = function(length, str) 
	{
		return this.pad(length, str, 1);	
	};
	obj.padRight = function(length, str) 
	{
		return this.pad(length, str, -1);
	};
    obj.reverse = function()
    {
        var r = this.match(/.|^s/gi);
        if(!r) return "";
        return r.reverse().join("");
    };
    obj.toNumber = function()
    {
        return Number.parse(this);
    };
    obj.isNumber = function()
    {
        return Number.isNumber(this);
    };
    obj.toInteger = function(radix)
    {
        return Number.parseInt(this, radix);
    };
    obj.isInteger = function(radix)
    {
        return Number.isInteger(this, radix);
    };
    obj.toDateTime = function(fmtStr)
    {
        if(!fmtStr)
            return new Date(this);
        return Date.parse(fmtStr, this);
    };
	obj.removeSpaces = function()
	{
		return this.replace(/ /ig,"");
	};
	obj.removeExtraSpaces=function()
	{
		return this.replace(String.prototype.removeExtraSpaces.regExp," ");
	};
	obj.removeExtraSpaces.regExp = new RegExp("\\s+","g");
	obj.removeSpaceDelimitedString=function(r)
	{
		var s=" "+this.trim()+" ";
		return s.replace(" "+r+" "," ").trim();
	};
	obj.encodeURI=function()
	{
		return escape(this).replace(/\+/g,"%2B");
	};
	obj.encodeHtml=function()
	{
		return this.replace(/\>/g,"&gt;").replace(/\</g,"&lt;").replace(/\&/g,"&amp;").replace(/\'/g,"&#039;").replace(/\"/g,"&quot;");
	};
	obj.decodeURI=function()
	{
		return unescape(this);
	};
	obj.format = function()
	{
		var params = [];
		params.push(this);
		for(var i=0; i<arguments.length; i++)
			params.push(arguments[i]);
		return String.format.apply(this, params);
	};
	obj.capitalize = function()
	{
		var words = this.split(" ");
		for(var i=0; i<words.length; i++)
			words[i] = words[i].charAt(0).toUpperCase() + words[i].substring(1);
		return words.join(" ");
	};
})();
/*RegExp*/
(function()
{
	var obj = RegExp;
	obj._typeName = "RegExp";
	obj.parse=function(str)
	{
		str = new String(str);
		if(str.startsWith("/"))
		{
			var endSlashIndex=str.lastIndexOf("/");
			if(endSlashIndex>1)
			{
				var expression=str.substring(1,endSlashIndex);
				var flags=str.substr(endSlashIndex+1);
				return new RegExp(expression,flags);
			}
		}
		return null;
	};
})();
/*Math*/
(function()
{
	var obj = Math;
	obj.div = function(a,b)
	{
		return (a-(a%b))/b;
	};
	obj.round2 = function(value, fractions)
	{
		if(!fractions) return Math.round(value);
		if(fractions > 0) return Math.round(value*Math.pow(10,fractions))/Math.pow(10,fractions);
		if(fractions < 0) return Math.round(value/Math.pow(10,fractions))*Math.pow(10,fractions);
	};
	obj.truncate = function(value, fractions)
	{
		if(value==0) return;
		if(value > 0)
		{
			if(!fractions) return Math.floor(value);
			if(fractions > 0) return Math.floor(value*Math.pow(10,fractions))/Math.pow(10,fractions);
			if(fractions < 0) return Math.floor(value/Math.pow(10,fractions))*Math.pow(10,fractions);
		}
		else
		{
			if(!fractions) return Math.ceil(value);
			if(fractions > 0) return Math.ceil(value*Math.pow(10,fractions))/Math.pow(10,fractions);
			if(fractions < 0) return Math.ceil(value/Math.pow(10,fractions))*Math.pow(10,fractions);
		}
	};
	obj.root = function(x, xp) {return Math.pow(x, 1/xp);};
	obj.cbrt = function(x) {return Math.pow(x, 1/3);};
	obj.logn = function(x, bs) {return Math.log(x)/Math.log(bs);};
	obj.ln = function(x) {return Math.log(x);};
	obj.lg = function (x) {return Math.log(x)/Math.LN10;};
	obj.lb = function(x) {return Math.log(x)/Math.LN2;};
	obj.cot = function(x) {return Math.cos(x)/Math.sin(x);};
	obj.sec = function(x) {return 1/Math.cos(x);};
	obj.csc = function(x) {return 1/Math.sin(x);};
	obj.acot = function(x) {return Math.atan(-x)+Math.PI/2;};
	obj.asec = function(x) {return Math.acos(1/x);};
	obj.acsc = function(x) {return Math.asin(1/x);};
	obj.sinh = function(x) {return (Math.exp(x)-Math.exp(-x))/2;};
	obj.cosh = function(x) {return (Math.exp(x)+Math.exp(-x))/2;};
	obj.sech = function(x) {return 2/(Math.exp(x)+Math.exp(-x));};
	obj.csch = function(x) {return 2/(Math.exp(x)-Math.exp(-x));};
	obj.tanh = function(x) {return (Math.exp(2*x)-1)/(Math.exp(2*x)+1);};
	obj.coth = function(x) {return (Math.exp(2*x)+1)/(Math.exp(2*x)-1);};
	obj.asinh = function(x) {return Math.log(x+Math.sqrt(x*x+1));};
	obj.acosh = function(x) {return Math.log(x+Math.sqrt(x*x-1));};
	obj.atanh = function(x) {return Math.log((1+x)/(1-x))/2;};
	obj.acoth = function(x) {return Math.log((x+1)/(x-1))/2;};
	obj.asech = function(x) {return Math.log(1/x+Math.sqrt(1/(x*x)-1));};
	obj.acsch = function(x) {return Math.log(1/x+Math.sqrt(1/(x*x)+1));};
	obj.gaussd = function(x,m,s) {return 1/(s*2.5066282746310002)*Math.exp(-(x -= m)*x/(2*s*s));};
	obj.sgn = function(x) {return (x < 0)? -1 : ((x > 0)? 1 : 0);};
	obj.fact = function(x)
	{
		x = Math.round(Math.abs(x));
		if (x > 1) return x*this.fact(x-1);
		return 1;
	}; 
	obj.compare = function(x, y)
	{
		return x > y ? 1 : (x == y? 0 : -1);
	};
})();
/*Error*/
(function()
{
	var obj = Error;
	obj._typeName = "Error";
})();
/*Array*/
(function()
{
	var obj = Array;
	obj._typeName="Array";
	obj.parse=function(value)
	{
//		1.value should be "[1,2,3]"
		return eval("("+value+")");
	};
	obj._interfaces=[];
	obj = obj.prototype;
	obj.add = obj.queue = function(value)
	{
		if(arguments.length == 1)
		{
			this.push(value);
			return;
		}
		for(var i = 0; i< arguments.length; i++)
			this.push(arguments[i]);
	};
	obj.addRange = function(arrayValue)
	{
		if(arrayValue.length != 0)
		{
			for(var i=0; i < length; i++)
			{
				this.push(arrayValue[i]);
			}
		}
	};
	obj.clear = function()
	{
		if(this.length>0)
			this.splice(0, this.length);
	};
	obj.clone = function()
	{
		var ret = [];
		var length=this.length;
		for(var i = 0; i < length; i++)
			ret[i]=this[i];
		return ret;
	};
	obj.contains = obj.exists = function(value)
	{
		var index=this.indexOf(value);
		return index >= 0;
	};
	obj.dequeue=function()
	{
		return this.shift();
	};
	if(!obj.indexOf)
	{
		obj.indexOf = function(value, startIndex)
		{
			var length=this.length;
			if(length!=0)
			{
				startIndex = startIndex || 0;
				if(startIndex < 0)
					startIndex = Math.max(0,length + startIndex);
				for(var i = startIndex; i<length; i++)
				{
					if(this[i] == value)
						return i;
				}
			}
			return -1;
		};
	}
	if(!obj.forEach)
	{
		obj.forEach=function(callbackMethod, caller)
		{
//			1.	callbackMethod: 3 parameters will be transfered to callbackMethod function
//				value, index of value, reference of array
			var length=this.length;
			for(var i=0; i<length; i++)
				callbackMethod.call(caller, this[i] , i , this);
		};
    }
	obj.insert=function(index, value)
	{
		this.splice(index, 0, value);
	};
	obj.remove=function(value)
	{
		var index = this.indexOf(value);
		if(index>=0)
			this.splice(index,1);
		return index >= 0;
	};
	obj.removeAt=function(index)
	{
		return this.splice(index,1)[0];
	};	
	obj.get_length=function()
	{
		return this.length;
	};
	obj.getItem=function(index)
	{
		return this[index];
	};
})();
/*Sys.Error*/
(function()
{
	var obj = Type.createClass("Sys.Error");
	obj.sysMessages = Sys.Resources.sysMessages;
	obj = obj.prototype;
	obj.number = 5000;
	obj.message = "Error!";
	obj.description = "Error!";
	obj.create = function()
	{
		var msg = Sys.Error.sysMessages;
		if(arguments.length==0)
			return;
		if(arguments.length==1 && typeof(arguments[0])=="number")
		{
			this.number = arguments[0];
			this.message = msg[arguments[0]][0];
			this.description = msg[arguments[0]][1];
			return;
		}
		if(typeof(arguments[0])=="number") 
		{
			this.number = arguments[0];
			this.description = msg[arguments[0]][1];
			arguments[0] = msg[arguments[0]][0];
			this.message = String.format.apply(this, arguments);
			return
		}
		this.message = String.FormatString(arguments);
	};
	obj.toString = function()
	{
		return this.message;
	};
})();
/*Sys.IDisposable*/
(function()
{
	var obj = Type.createInterface("Sys.IDisposable");
	obj = obj.prototype;
	obj.dispose = Type.abstractMethod; 
})();
/*Sys.Enum*/
(function()
{
	var obj = registerNamespace("Sys.Enum");
	obj.create = Type.createSimpleAnonymousEnum;
	obj.extend = function(baseEnum, additionalEnum)
	{
		if(!baseEnum)
			baseEnum = Sys.Enum.create();
		return baseEnum.extend(additionalEnum);
	};
	obj.getValue = function(Enum, str)
	{
		return Enum.parse(str);
	};
})();
/*Sys.Flags*/
(function()
{
	var obj = registerNamespace("Sys.Flags");
	obj.create = Type.createSimpleAnonymousFlags;
})();
/*Sys.StringBuilder*/
(function()
{
	var obj = Type.createSealedClass("Sys.StringBuilder", "Sys.IDisposable");
	obj = obj.prototype;
	obj.create = function()
	{	
		this._data = [];
		this.append.apply(this, arguments)
	};
	obj.dispose = function()
	{
		if(this._data)
		{
			for(var i = this._data.length - 1; i >= 0; i--)
				this._data[i] = null;
			delete this._data;
		};
	};
	obj.append=function()
    {
        for(var i=0; i<arguments.length; i++)
		{
			if(typeof(arguments[i])=="string" && arguments[i].length!=0)
				this._data.push(arguments[i]);
        }
    };
    obj.appendLine=function()
    {
        for(var i=0; i<arguments.length; i++)
		{
			if(typeof(arguments[i])=="string" && arguments[i].length!=0)
				this._data.push(arguments[i]);
			this._data.push("\r\n");
        }
    };
    obj.clear=function()
    {
        this._data.clear();
    };
    obj.isEmpty=function()
    {
        return this._data.length==0;
    };
    obj.toString=function(separator)
    {
        return this._data.join(separator||"");
    };
})();
/*Sys.IArray*/
(function()
{
	var obj = Type.createInterface("Sys.IArray");
	Array._interfaces.push(obj);
	obj = obj.prototype;
	obj.get_length = Type.abstractMethod;
    obj.getItem= Type.abstractMethod; 
})();

/*Sys.ICloneable*/
(function()
{
	var obj = Type.createInterface("Sys.ICloneable");
	Array._interfaces.push(obj);
	obj = obj.prototype;
	obj.clone = Type.abstractMethod; 
})();
/*Sys.Event*/
(function()
{
	var obj = Type.createClass("Sys.Event", Sys.IDisposable);
	obj = obj.prototype;
	obj._handlers = null;
	obj.create = function()
	{
		this._handlers = [];
	};
	obj.dispose = function()
	{
		var h = this._handlers;
		if(h)
		{
			for(var i = h.length - 1; i > 0; i--)
				h[i] = null;
		}
		this._handlers = null
	};
	obj.clear = function()
	{
		this._handlers.clear();
	};
	obj.isActive = function()
	{
		return this._handlers != null && this._handlers.length != 0;
	};
	obj.add = obj.attach = function(func)
	{
		if(!this._handlers.exists(func))
		{
			this._handlers.queue(func);
			return true;
		};
		return false;
	};
	obj.remove = obj.detach = function(func)
	{
		return this._handlers.remove(func);
	};
	obj.fire = function()
	{
		if(!this.isActive()) return;
		var ret;
		if(arguments.length == 0)
		{
			for(var i = 0; i < this._handlers.length; i++)
			{
				ret = this._handlers[i].apply(this);
				if(typeof(ret) != "undefined" && !ret) break;
			}
		}
		else
		{
			for(var i = 0; i < this._handlers.length; i++)
			{
				r = this._handlers[i].apply(this, arguments);
				if(typeof(ret) != "undefined" && !ret) break;
			}
		}
		return ret;
	};
})();
/*Sys.Timer*/
(function()
{
	var obj = Type.createSealedClass("Sys.Timer", Sys.IDisposable);
	obj = obj.prototype;
	obj.onTimer = null;
	obj.create = function()
	{
		obj.onTimer = new Sys.Event();
		var pr = this._();
		pr.interval = 1000;
		pr.handle = null;
		pr.enabled = false;
	};
	obj.dispose = function()
	{
		this.stop();
		this.onTimer.dispose();
	};
	obj.get_Interval = function()
    {
        return this._().interval;
    };
    obj.set_Interval = function(value)
    {
		var pr = this._();
        if(value <= 0 )
            throw new Sys.Error(1004);
        pr.interval = Math.round(value);
        if(pr.enabled)
        {
            this.stop();
            this.start();
        }
    };
    obj.get_Enabled = function()
    {
        return this._().enabled;
    };
    obj.set_Enabled = function(value)
    {
        if(value)
            this.start();
        else
            this.stop();            
    };
    obj.start = function()
    {
		var pr = this._();
        if(pr.enabled) return;
        pr.handle = window.setInterval(this.onTimer.fire, pr.interval);
        pr.enabled = true;
    };
    obj.stop = function()
    {
		var pr = this._();
        if(!pr.enabled) return;
        window.clearInterval(pr.handle);
        pr.handle = null;
        pr.enabled = false;
    };
})();
/*Sys.Delay*/
(function()
{
	var obj = Type.createSealedClass("Sys.Delay", Sys.IDisposable);
	obj = obj.prototype;
	obj.onDelay = null;
	obj.interval = 1000;
	obj._handle = 0;
	obj.create = function()
	{
		obj.onDelay = new Sys.Event();
	};
	obj.dispose = function()
	{
		this.onDelay.dispose();
	};
    obj.start = function()
    {
		if(this._interval <= 0 )
            throw new Sys.Error(1004);
        this._handle = window.setTimeout(Type.createDelegate(this, function(){this.onDelay.fire();this.stop();}), this._interval);
    };
    obj.stop = function()
    {
		if(this._handle==0) return;
		try
		{
			window.clearTimeout(this._handle);
		}
		catch(ee){}
		finally
		{
			this._handle = 0;
		};
    };
})();

/*
Sys.AssignValues = function(obj)
{
    var ret = new Object();
    for(var i in obj)
        if(typeof(obj[i]) == "number" || typeof(obj[i]) == "string" || typeof(obj[i]) == "boolean" )
            ret[i] = obj[i];
    return ret;
};
Sys.IsEmptyObject = function(obj)
{
    for(var i in obj)
        return false;
    return true;
};
*/
/*TList*/
/*
Sys.TList = Sys.TObject.SubClass();
Sys.TList.CreateClass = function()
{
    obj = this.prototype;
    obj._Buffer = null;
    obj._Current = -1;
    obj._Bof = true;
    obj._Eof = true;
    obj.Contains = function(obj)
    {
        return this._Buffer.Contains(obj);
    };
    obj.getCount = function()
    {
        return this._Buffer.length;
    };
    obj.getItem = function(Index)
    {
        this.__CheckBoundary(Index, 0);
        return this._Buffer[Index];
    };
    obj.setItem = function(Index, Value)
    {
        this.__CheckBoundary(Index, 0);
        this._Buffer[Index] = Value;
    };
    obj.__CheckBoundary = function(Index, Type, Count)
    {
        if(Type == 0)
            if(!this._Buffer[Index])
                throw Error.Create(10000); //TList out of bound.
        if(Type == 1)
        {
            if(!Count)
                Count = 1;
            if((Index>=0)&&((!this._Buffer[Index])||(!this._Buffer[Index+Count-1])))
                throw Error.Create(10000); //TList out of bound.
            if((Index<0)&&((!this._Buffer[this._Buffer.length + Index])||(!this._Buffer[this._Buffer.length + Index + Count-1])))
                throw Error.Create(10000); //TList out of bound.
        }
    };
    obj.DefProp("UseCompareFunction", false);
    obj.IndexOf = function(Item)
    {
        for(var i in this._Buffer)
        {
            if(!this.getUseCompareFunction())
            {
                if(this._Buffer[i] == Item)
                    return i;
            }
            else
            {
                var fn = this.getCompareFunction();
                if(typeof(fn)!="function")
                {
                    if(this._Buffer[i] == Item)
                    return i;
                }
                else
                {
                    if(fn.call(this, Item,this._Buffer[i])==0)
                        return i;
                }
            }
        }
        return -1;
    };
    obj.Clear = function()
    {
        this._Buffer = new Array();
        this.__ValidatePointer();
    };
    obj.Add = function()
    {
        try
        {
            for(var i = 0; i<arguments.length; i++)
                this._Buffer.push(arguments[i]);
        }
        finally
        {
            this.__ValidatePointer();
        }
    };
    obj.DeleteAt = function(Index, Count)
    {
        if(!Count)
            Count = 1;
        try
        {
            this.__CheckBoundary(Index, 1, Count);
            this._Buffer.splice(Index, Count);
        }
        finally
        {
            this.__ValidatePointer();
        }
    };
    obj.InsertAt = function(Index)
    {
        try
        {
            this.__CheckBoundary(Index, 1);
            if(arguments.length <= 1 )
                return;
            if(Index >= 0)
                for(var i = arguments.length - 1; i > 0 ; i--)
                    this._Buffer.splice(Index, 0, arguments[i]);
            else
                for(var i = 0; i < arguments.length; i++)
                    this._Buffer.splice(Index, 0, arguments[i]);
        }
        finally
        {
            this.__ValidatePointer();
        }
    };
    obj.Reverse = function()
    {
        this._Buffer.reverse();
        this.First();
    };
    obj.DefProp("CompareFunction", null);
    obj.Sort = function()
    {
        if(this.getUseCompareFunction() &&(typeof(this.getCompareFunction()) == "function"))
            this._Buffer.sort(this._CompareFunction);
        else
            this._Buffer.sort();
        this.First();
    };
    obj.Slice = function(Start, End)
    {
        this.__CheckBoundary(Start, 1);
        var ret = Sys.TList.Create();
        if(End)
        {
            this.__CheckBoundary(End, 1);
            ret._Buffer = this._Buffer.slice(Start, End);
        }
        else
            ret._Buffer = this._Buffer.slice(Start);
        return ret;
    };
    obj.Shift = function()
    { 
        var ret = this._Buffer.shift();
        this.__ValidatePointer();
        return ret;
    };
    obj.Unshift = function()
    {
        try
        {
            if(arguments.length == 0)
                return;
            for(var i = 0; i < arguments.length; i++)
                this._Buffer.unshift(arguments[i]);
        }
        finally
        {
            this.__ValidatePointer();
        }
    };
    obj.DefProp("ActAs","Stack");//Stack or Queue
    obj.Peek = function(Type)
    {
        if(this._Buffer.length == 0)
            return null;
        var mType = "STACK"
        if(!Type)
            mType = this._ActAs.toUpperCase();
        else
            mType = Type.toUpperCase();
        if(mType == "STACK" )
            return this._Buffer[this._Buffer.length-1];
        else
            return this._Buffer[0];
    };
    obj.Push = function()
    {
        try
        {
            if(this._Buffer.length == 0)
                return null;
            for(var i = 0; i<arguments.length; i++)
                this._Buffer.push(arguments[i]);
        }
        finally
        {
            this.__ValidatePointer();
        }
    };
    obj.Pop = function(Type)
    {
        try
        {
            if(this._Buffer.length == 0)
                return null;
            var mType = "STACK"
            if(!Type)
                mType = this._ActAs.toUpperCase();
            else
                mType = Type.toUpperCase();
            if(mType == "STACK" )
                return this._Buffer.pop();
            else
                return this._Buffer.shift();
        }
        finally
        {
            this.__ValidatePointer();
        }
    };
    obj.AtLeast = function(Count)
    {
        return (this._Buffer.length >= Count);
    };
    obj.__ValidatePointer = function()
    {
        if(this._Buffer.length == 0)
        {
            this._Eof = true;
            this._Bof = true;
            this._Current = -1;
            return;
        }
        if(this._Current>=this._Buffer.length)
        {
            this._Eof = true;
            this._Bof = false;
            this._Current = this._Buffer.length ;
            return;
        }
        if(this._Current <0)
        {
            this._Eof = false;
            this._Bof = true;
            this._Current = -1;
            return;
        }
        this._Eof = false;
        this._Bof = false;
    };
    obj.__GetIndex = function()
    {
        this.__ValidatePointer();
        var Index = this._Current;
        if(Index >= this._Buffer.length ) Index = this._Buffer.length -1;
        if(Index < 0) Index = 0;
        return Index;
    };
    obj.First = function()
    {
        this._Current = -1;
        this.__ValidatePointer();
    };
    obj.Prior = function()
    {
        this._Current = this.__GetIndex() -1;
        this.__ValidatePointer();
    };
    obj.Next = function()
    {
        this._Current = this.__GetIndex() + 1;
        this.__ValidatePointer();
    };
    obj.Last = function()
    {
        this._Current = this._Buffer.length;
        this.__ValidatePointer();
        return;
    };
    obj.Insert = function()
    {
        var Index = this.__GetIndex() ;
        if(Index == 0)
        {
            this._Buffer.unshift(arguments[0]);
            this._Current = 0;
        }
        else
        {
            this._Buffer.splice(Index, 0, arguments[0]);
            this._Current = Index;
        }
        this.__ValidatePointer();
        return;
    };
    obj.Append = function()
    {
        this._Buffer.push(arguments[0]);
        this._Current = this._Buffer.length -1;
        this.__ValidatePointer();
    };
    obj.Delete = function()
    {
        if(this._Buffer.length == 0) return;
        var Index = this.__GetIndex() ;
        this._Buffer.splice(Index,1);
        this._Current = Index;
        this.__ValidatePointer();
    };
    obj.getCurrent = function()
    {
        var Index = this.__GetIndex() ;
        return this._Buffer[Index];
    };
    obj.Value = function() //current, same as getCurrent;
    {
        return this.getCurrent();
    };
    obj.setCurrent = function(Value)
    {
        var Index = this.__GetIndex() ;
        if(this._Buffer.length == 0) this.Insert(Value);
        this._Buffer[Index] = Value;
    };
    obj.MoveBy = function(Count)
    {
        var Index = this.__GetIndex() ;
        this._Current = Index + Count;
        this.__ValidatePointer();
    };
    obj.GotoBookmark = function(Bookmark)
    {
        this.__CheckBoundary(Bookmark, 0);
        this._Current = Bookmark;
        this.__ValidatePointer();
    };
    obj.GetBookmark = function()
    {
        return this.__GetIndex();
    };
    obj.Swap = function(IndexA, IndexB)
    {
        this.__CheckBoundary(IndexA, 0);
        this.__CheckBoundary(IndexB, 0);
        var c = this._Buffer[IndexA];
        this._Buffer[IndexA] = this._Buffer[IndexB];
        this._Buffer[IndexB] = c;
        c = null;
    };
    obj.getEof = function()
    {
        this.__ValidatePointer();
        return this._Eof;
    };
    obj.getBof = function()
    {
        this.__ValidatePointer();
        return this._Bof;
    };
    obj.ForEach = function(This, Handle)
    {
        if(typeof(Handle)!="function")
            return;
        for(var i in this._Buffer)
            Handle.call(This, this._Buffer[i]);
    };
    obj.Assign = function() 
    {   
        var op="COPY";
        var StartFrom = 0;
        var LSource = arguments[0];
        if(typeof(arguments[0]) =="string")
        {
            op = arguments[0].toUpperCase();
            StartFrom = 1;
            LSource = arguments[1];
        }
        if(arguments.length>2)
            for(var i = StartFrom; i < arguments.length; i++)
                this.Assign(op, arguments[i]);
        if(op == "ADD") //12345, 346 = 12345346
        {
            this._Buffer.push.apply(this._Buffer, LSource._Buffer);
            this.__ValidatePointer();
            return;
        }
        if(op == "COPY")// 12345, 346 = 346 : only those in the new list
        {
            this._Buffer = LSource._Buffer.slice(0);
            this.__ValidatePointer();
            return;
        }
        if(op == "AND") // 12345, 346 = 34 : intersection of the two lists
        {
            for(var i = this._Buffer.length -1; i >= 0 ; i--)
            {
                if(LSource.IndexOf(this._Buffer[i]) == -1) 
                    this._Buffer.splice(i, 1);
            }
            this.__ValidatePointer();
            return;
        }
        if(op == "OR")// 12345, 346 = 123456 : union of the two lists
        {
            for(var i in LSource._Buffer)
            {
                if(this.IndexOf(LSource._Buffer[i]) == -1)
                    this._Buffer.push(LSource._Buffer[i]);
            }
            this.__ValidatePointer();
            return;
        }
        if(op == "XOR")// 12345, 346 = 1256 : only those not in both lists
        {
            var LTemp = Sys.TList.Create();
            try
            {
                for(var i in LSource._Buffer)
                    if(this.IndexOf(LSource._Buffer[i]) == -1)
                        LTemp._Buffer.push(LSource._Buffer[i]);
                for(var i = this._Buffer.length -1; i >= 0 ; i--)
                    if(LSource.IndexOf(this._Buffer[i]) != -1)
                        this._Buffer.splice(i,1);
                LTemp.ForEach(this, (function(item){this._Buffer.push(item);}));    
            }
            finally
            {
                LTemp = null;
            }
            this.__ValidatePointer();
            return;
        }
        // 12345, 346 = 125 : only those unique to source
        if(op == "SRCUNIQUE")
        {
            for(var i = this._Buffer.length -1; i >= 0 ; i--)
                if(LSource.IndexOf(this._Buffer[i]) != -1)
                    this._Buffer.splice(i,1);
            this.__ValidatePointer();
            return;
        }
        // 12345, 346 = 6 : only those unique to dest
        if(op == "DESTUNIQUE")
        {
            var LTemp = Sys.TList.Create();
            try
            {
                for(var i in LSource._Buffer)
                    if(this.IndexOf(LSource._Buffer[i]) == -1)
                        LTemp._Buffer.push(LSource._Buffer[i]);
                this.Assign("COPY", LTemp);
            }
            finally
            {
                LTemp = null;
                this.__ValidatePointer();
                return;
            }
        }
    };
    obj.Create = function()
    {
        if(arguments.length>0)
            this._Buffer = Sys.ArgumentsToArray(arguments);
        else
            this._Buffer = new Array();
        this.__ValidatePointer();
    };
};
Sys.TList.CreateClass();


*/