/*
    Author:         John Huang
    Start Date:     2006-04-25 
    Description:    Initialize the native objects and create common procedures
*/
function DoNothing(){return;}
Sys.ArgumentsToArray = function(arg)
{
    var r = new Array();
    for(var i=0; i<arg.length; i++)
        r[i] = arg[i];
    return r;
};
Sys.InstanceOf = function(Instance, Class) 
{
    if(!Instance)
        return false;
    if(!Class)
        return false;
    if(Instance instanceof Class)
        return true;
    for(var i in Instance.BaseClasses)
    {
        if(arguments.callee(Instance.BaseClasses[i], Class))
            return true;
    }
    return false;
};
Sys.InternalIDGenerator = function()
{
    var obj = arguments.callee;
    if(!obj.InternalIDCounter)
      obj.InternalIDCounter=0;
    return (obj.InternalIDCounter++);
};
Sys.GenerateAnonymous = function()
{
    return "sYs__" + Sys.InternalIDGenerator().toString();
};
/*Initialize the native objects*/
/*Boolean*/
Boolean.Format = function(Format, B)
{
    var r = Format.indexOf(";");
    if(r==-1) return Format;
    return (B==true? Format.Left(r):Format.Right(Format.length-r-1));
};
Boolean.ToInteger = function(Value)
{
    return Value ? 1 : 0;
};
(function()
{
    var obj = Boolean.prototype;
    obj.Format = function(Format)
    {
        return Boolean.Format.call(this, Format, B);
    };
    obj.ToInteger = function()
    {
        return Boolean.ToInteger(this);
    }
})();
/*Date*/
Date.CurrentFormat = function()
{
    return Sys.Resources.DateTimeFormat[Sys.Resources.DateTimeFormat.Current];
};
Date.DaysInAMonth = function(Year, Month)
{
    if(Month==3||Month==5||Month==8||Month==10)
        return 30;
    if(Month==1)
        return Date.IsLeapYear(Year) == true? 29:28;
    return 31;
};
Date.DaysInAYear = function(Year)
{
    return Date.IsLeapYear(Year)==true? 366:365;
};
Date.IsLeapYear = function(Year)
{
    return (Year % 4 == 0) && ((Year % 100 != 0) || (Year % 400 == 0));
};
Date.Parse = function(Format, Str)
{
    var Style = Date.CurrentFormat();
    var year = 0, month = 0, day = 0, hour = 0, minute = 0, second = 0, millisecond = 0, a = -1;
    if(!Format)
      return Format = Style.Default;
      
    var DatetimeFormatToArray = function(fmt)
    {
        var ret =[], i = -1, j, k;
        var Styles = Sys.Resources.DateTimeFormat.Formats;
        var sortfunc = function(a,b)
        {
            if(a.Index > b.Index) return 1;
            if(a.Index < b.Index) return -1;
            if(a.Index == b.Index) return 0;
        }
        fmt = fmt.toLowerCase();
        for(j in Styles)
        {
            for(k in Styles[j])
            {
                i = fmt.indexOf(Styles[j][k]);
                if(i > -1)
                {
                    ret.push({Name: j, Index : i, Length : Styles[j][k].length, Format : Styles[j][k]});
                    break;
                }
            }
        }
        ret.sort(sortfunc);
        return ret;
    };
    var AFormat = DatetimeFormatToArray(Format);
    var ArrayIndexOf = function(a, index)
    {
        for(var z in a)
            if(a[z].toLowerCase() == Str.substr(index,a[z].length).toLowerCase())
                return z;
        return -1;
    }
    var f_pre_pos = 0, f_pre_len = 0, f_gap = 0, obj = null;
    var r_pre_pos = 0, r_cur_pos = 0, r_pre_len = 0;
    for(var idx in AFormat)
    {
        obj = AFormat[idx];
        f_gap = obj.Index - f_pre_len - f_pre_pos;                
        r_cur_pos = r_pre_pos + r_pre_len + f_gap;
        switch(obj.Name) 
        {
            case "Year":
                switch(obj.Format)
                {
                    case "yy":
                        year = ("20"+Str.substr(r_cur_pos, 2)).ToInteger();
                        r_pre_pos = r_cur_pos; r_pre_len = 2;
                        break;
                    case "yyyy":
                        year = Str.substr(r_cur_pos, 4).ToInteger();
                        r_pre_pos = r_cur_pos; r_pre_len = 4;
                }
                break;
            case "Month":
                switch(obj.Format)
                {
                    case "m":
                        if(Str.substr(r_cur_pos,1).IsInteger()&&Str.substr(r_cur_pos+1,1).IsInteger())
                        {
                            month = Str.substr(r_cur_pos,2).ToInteger();
                            if(month <= 12)
                            {
                                r_pre_pos = r_cur_pos; r_pre_len = 2; month --;
                                break;
                            }
                        }
                        month = Str.substr(r_cur_pos,1).ToInteger();
                        r_pre_pos = r_cur_pos; r_pre_len = 1; month --;
                        break;
                    case "mm":
                        month = Str.substr(r_cur_pos,2).ToInteger();
                        r_pre_pos = r_cur_pos; r_pre_len = 2; month --;
                        break;
                    case "mmm":
                        month = ArrayIndexOf.call(Str, Style.ShortMonths,r_cur_pos);
                        if(month < 0)
                            throw "Could not convert Month";
                        r_pre_pos = r_cur_pos; r_pre_len = Style.ShortMonths[month].length;
                        break;
                    case "mmmm":
                        month = ArrayIndexOf.call(Str, Style.LongMonths,r_cur_pos);
                        if(month < 0)
                            throw "Could not convert Month";
                        r_pre_pos = r_cur_pos; r_pre_len = Style.LongMonths[month].length;
                        break;
                }
                break;
            case "WeekDay":
                switch(obj.Format)
                {
                    case "wk":
                        var t = ArrayIndexOf.call(Str,Style.ShortWeekdays,r_cur_pos);
                        if(t < 0)
                            throw "Could not convert week day";
                        r_pre_pos = r_cur_pos; r_pre_len = Style.ShortMonths[t].length;
                        break;
                    case "wkl":
                        var t = ArrayIndexOf.call(Str, Style.LongWeekdays,r_cur_pos);
                        if(t < 0)
                            throw "Could not convert week day";
                        r_pre_pos = r_cur_pos; r_pre_len = Style.LongWeekdays[t].length;
                        break;
                }
                break;
            case "Day":
                switch(obj.Format)
                {
                    case "d":
                        if(Str.substr(r_cur_pos,1).IsInteger()&&Str.substr(r_cur_pos+1,1).IsInteger())
                        {
                            day = Str.substr(r_cur_pos,2).ToInteger();
                            r_pre_pos = r_cur_pos; r_pre_len = 2;
                            break;
                        }
                        day = Str.substr(r_cur_pos,1).ToInteger();
                        r_pre_pos = r_cur_pos; r_pre_len = 1; 
                        break;
                    case "dd":
                        day = Str.substr(r_cur_pos,2).ToInteger();
                        r_pre_pos = r_cur_pos; r_pre_len = 2;
                        break;
                }
                break;
            case "Hour":
                switch(obj.Format)
                {
                    case "h":
                        if(Str.substr(r_cur_pos,1).IsInteger()&&Str.substr(r_cur_pos+1,1).IsInteger())
                        {
                            hour = Str.substr(r_cur_pos,2).ToInteger();
                            r_pre_pos = r_cur_pos; r_pre_len = 2;
                            break;
                        }
                        hour = Str.substr(r_cur_pos,1).ToInteger();
                        r_pre_pos = r_cur_pos; r_pre_len = 1; 
                        break;
                    case "hh":
                        hour = Str.substr(r_cur_pos,2).ToInteger();
                        r_pre_pos = r_cur_pos; r_pre_len = 2;
                        break;
                }
                break;
            case "Minute":
                switch(obj.Format)
                {
                    case "n":
                        if(Str.substr(r_cur_pos,1).IsInteger()&&Str.substr(r_cur_pos+1,1).IsInteger())
                        {
                            minute = Str.substr(r_cur_pos,2).ToInteger();
                            r_pre_pos = r_cur_pos; r_pre_len = 2;
                            break;
                        }
                        hour = Str.substr(r_cur_pos,1).ToInteger();
                        r_pre_pos = r_cur_pos; r_pre_len = 1; 
                        break;
                    case "nn":
                        minute = Str.substr(r_cur_pos,2).ToInteger();
                        r_pre_pos = r_cur_pos; r_pre_len = 2;
                        break;
                }
                break;
            case "Second":
                switch(obj.Format)
                {
                    case "s":
                        if(Str.substr(r_cur_pos,1).IsInteger()&&Str.substr(r_cur_pos+1,1).IsInteger())
                        {
                            second = Str.substr(r_cur_pos,2).ToInteger();
                            r_pre_pos = r_cur_pos; r_pre_len = 2;
                            break;
                        }
                        second = Str.substr(r_cur_pos,1).ToInteger();
                        r_pre_pos = r_cur_pos; r_pre_len = 1; 
                        break;
                    case "ss":
                        second = Str.substr(r_cur_pos,2).ToInteger();
                        r_pre_pos = r_cur_pos; r_pre_len = 2;
                        break;
                }
                break;
            case "Millisecond":
                switch(obj.Format)
                {
                    case "ms":
                        if(Str.substr(r_cur_pos,1).IsInteger()&&Str.substr(r_cur_pos+1,1).IsInteger()&&Str.substr(r_cur_pos+2,1).IsInteger())
                        {
                            millisecond = Str.substr(r_cur_pos,3).ToInteger();
                            r_pre_pos = r_cur_pos; r_pre_len = 3;
                            break;
                        }
                        if(Str.substr(r_cur_pos,1).IsInteger()&&Str.substr(r_cur_pos+1,1).IsInteger())
                        {
                            millisecond = Str.substr(r_cur_pos,2).ToInteger();
                            r_pre_pos = r_cur_pos; r_pre_len = 2;
                            break;
                        }
                        millisecond = Str.substr(r_cur_pos,1).ToInteger();
                        r_pre_pos = r_cur_pos; r_pre_len = 1; 
                        break;
                    case "ms1":
                        millisecond = Str.substr(r_cur_pos,2).ToInteger();
                        r_pre_pos = r_cur_pos; r_pre_len = 3;
                        break;
                }
                break;
            case "A":
                switch(obj.Format)
                {
                    case "am/pm":
                        var a = ArrayIndexOf.call(Str, Style.AM_PM,r_cur_pos);
                        if(a < 0)
                            throw "Could not convert AM/PM";
                        r_pre_pos = r_cur_pos; r_pre_len = Style.AM_PM[a].length;
                        break;
                    case "a/p":
                        var t = ArrayIndexOf.call(Str, Style.A_P,r_cur_pos);
                        if(t < 0)
                            throw "Could not convert AM/PM";
                        r_pre_pos = r_cur_pos; r_pre_len = Style.A_P[a].length;
                        break;
                }
                break;
        }
        f_pre_len = obj.Length; 
        f_pre_pos = obj.Index
    }
    if(a == 0 && hour == 12) hour = 0;
    if(a == 1 && hour < 12) hour = hour + 12;
    return new Date(year, month, day, hour, minute, second, millisecond);
};
Date.Format = function(Format, Dt)
{
    Dt = new Date(Dt.valueOf());
    var r = Sys.Resources.DateTimeFormat.RegularExpression, i, j, k;
    var h = Dt.getHours();
    var Style = Date.CurrentFormat();
    r = Format.match(r);
    if(!r) return Format;
    var is12 = Sys.Resources.DateTimeFormat.RegularExpressionIs12.test(Format);
    for(var i = 0; i <r.length; i ++)
    {
        switch(r[i].toLowerCase())
        {
            case "yyyy":
                r[i] = Dt.getFullYear().toString();
                break;
            case "yy":
                r[i] = Dt.getFullYear().toString().Right(2);
                break;
            case "mmmm":
                r[i] = Style.LongMonths[Dt.getMonth()];
                break;
            case "mmm":
                r[i] = Style.ShortMonths[Dt.getMonth()];
                break;
            case "mm":
                r[i] = ("0"+(Dt.getMonth()+1).toString()).Right(2);
                break;
            case "m":
                r[i] = (Dt.getMonth()+1).toString();
                break;
            case "wkl":
                r[i] = Style.LongWeekdays[Dt.getDay()];
                break;
            case "wk":
                r[i] = Style.ShortWeekdays[Dt.getDay()];
                break;
            case "dd":
                r[i] = ("0"+Dt.getDate().toString()).Right(2);
                break;
            case "d":
                r[i] = Dt.getDate().toString();
                break;
            case "hh":
                if(is12)
                {
                    if(h>12)
                        r[i] = ("0"+(h-12).toString()).Right(2);
                    else if(h >0 && h<= 12)
                        r[i] = ("0"+h.toString()).Right(2);
                    else 
                        r[i] = "00";
                }
                else
                    r[i] = ("0"+h.toString()).Right(2);
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
                r[i] = ("0"+Dt.getMinutes().toString()).Right(2);
                break;
            case "n":
                r[i] = Dt.getMinutes().toString();
                break;
            case "ss":
                r[i] = ("0"+Dt.getSeconds().toString()).Right(2);
                break;
            case "s":
                r[i] = Dt.getSeconds().toString();
                break;
            case "ms1":
                r[i] = ("00"+(Dt.getMilliseconds()).toString()).Right(3);
                break;
            case "ms":
                r[i] = Dt.getMilliseconds().toString();
                break;
            case "am/pm":
                r[i] = is12?( h<12? Style.AM_PM[0]:Style.AM_PM[1]):"";
                break;
            case "a/p":
                r[i] = is12?( h<12? Style.A_P[0]:Style.AM_PM[1]):"";
        }
    }
    return r.join("");
};
(function()
{
    var obj = Date.prototype;
    obj.Format = function(Format)
    {
        return Date.Format.call(this, Format, this);
    };
    obj.IsLeapYear = function()
    {
        return Date.IsLeapYear(this.getFullYear());
	};
	obj.DateOf = function()
	{
        return new Date(this.getFullYear(), this.getMonth(), this.getDate());
	};
	obj.TimeOf = function()
	{
        return new Date(0, 0, 0, this.getHours(),this.getMinutes(), this.getSeconds(), this.getMilliseconds());
	}; 
	obj.AddMilliseconds = function(Value)
	{
	    return new Date(this.valueOf()+Value);
	};
	obj.AddSeconds = function(Value)
	{
	    return new Date(this.valueOf()+Value*1000);
	};
	obj.AddMinutes = function(Value)
	{
	    return new Date(this.valueOf()+Value*60000);
	};
	obj.AddHours = function(Value)
	{
	    return new Date(this.valueOf()+Value*3600000);
	};
	obj.AddDays = function(Value)
	{
	    return new Date(this.valueOf()+86400000*Value);
	};
	obj.AddWeeks = function(Value)
	{
	    return new Date(this.valueOf()+604800000*Value);
	};
	obj.AddMonths = function(Value)
	{
	    var d0= this.getDate();
	    var date = new Date(this.getFullYear(),this.getMonth()+Value,1);
	    var d1 = Date.DaysInAMonth(date.getFullYear(), date.getMonth())
        date.setDate(d0<=d1 ? d0 : d1);
        date.setHours(this.getHours(), this.getMinutes(), this.getSeconds(), this.getMilliseconds());
        return date;
	};
	obj.AddYears = function(Value)
	{
	    return this.AddMonths(Value * 12);
	};
	obj.DiffMonth = function(to)
	{
	    return to.getFullYear()*12+to.getMonth()-this.getFullYear()*12 - this.getMonth();
	};
	obj.DiffDay = function(to)
	{
	    return Math.Div((to.valueOf() - this.valueOf()),86400000);
	};
	obj.DiffYear = function(to)
	{
	    return to.getFullYear() - this.getMonth();
	};
	obj.DiffWeek = function(to)
	{
	    return Math.Div((to.valueOf() - this.valueOf()),604800000);
	};
	obj.DiffMillisecond = function(to)
	{
	    return to.valueOf() - this.valueOf();
	};
	obj.DiffSecond = function(to)
	{
	    return Math.Div((to.valueOf() - this.valueOf()),1000);
	};
	obj.DiffMinute = function(to)
	{
	    return Math.Div((to.valueOf() - this.valueOf()),60000);
	};
	obj.DiffHour = function(to)
	{
	    return Math.Div((to.valueOf() - this.valueOf()),3600000);
	};
	obj.Diff = function(type, to) //type can be year/y, month/m, day/d, week/w, hour/h, minite/n, second/s, millisecond/ms
	{
	    type = (new String(type)).toLowerCase();
	    to = new Date(to.valueOf());
	    switch(type)
	    {
	        case "m":
	        case "month":
	            return this.DiffMonth(to);
	        case "d":
	        case "day":
	            return this.DiffDay(to);
	        case "y":
	        case "year":
	            return this.DiffYear(to);
            case "w":
	        case "week":
	            return this.DiffWeek(to);
	        case "ms":
	        case "millisecond":
	            return this.DiffMillisecond(to);
	        case "s":
	        case "second":
	            return this.DiffSecond(to);
	        case "n":
	        case "minute":
	            return DiffMinute(to);
	        case "h":
	        case "hour":
	            return DiffHour(to);
	    }
	}
})();
/*Error*/
Error.Create = function() //Can create an error like Create(msg), Create(num), Create(num,values....) Create(Format, values...)
{
    var r = new this();
    if(arguments.length==1)
    {
        if(typeof(arguments[0])=="number") 
        {
            r.number = arguments[0];
            r.message = Sys.Resources.SysMessages[arguments[0]][0];
            r.description = Sys.Resources.SysMessages[arguments[0]][1];
        }
        else
        {
            r.number = 50000;
            r.message = arguments[0].toString();
        }
    }
    else
    {
        if(typeof(arguments[0])=="number") 
        {
            var arg = new Array();
            r.number = arguments[0];
            arg.push(Sys.Resources.SysMessages[arguments[0]][0]);
            for(i=1; i< arguments.length; i++) arg.push(arguments[i]);
            r.message = String.FormatString(arg);
            r.description = Sys.Resources.SysMessages[arguments[0]][1];
        }
        else 
        {
            r.number = 50000;
            r.message = String.FormatString(arguments);
        }
    }
    return r;
};
(function()
{
    var obj = Error.prototype;
    if(!obj.message) obj.message = "";
    if(!obj.number) obj.number = 50000;
    if(!obj.description) obj.description = 0;
    if(!obj.tag) obj.tag = null;
    obj.toString = function(){return this.message;};
    obj.ToString = function(){return this.message;};

})();
/*Math*/
Math.Div = function(a,b)
{
    return (a-(a%b))/b;
};
Math.Round = function(Value, Fraction)
{
    if(!Fraction) return Math.round(Value);
    if(Fraction > 0) return Math.round(Value*Math.pow(10,Fraction))/Math.pow(10,Fraction);
    if(Fraction < 0) return Math.round(Value/Math.pow(10,Fraction))*Math.pow(10,Fraction);
};
Math.Truncate = function(Value, Fraction)
{
    if(Value==0) return;
    if(Value > 0)
    {
        if(!Fraction) return Math.floor(Value);
        if(Fraction > 0) return Math.floor(Value*Math.pow(10,Fraction))/Math.pow(10,Fraction);
        if(Fraction < 0) return Math.floor(Value/Math.pow(10,Fraction))*Math.pow(10,Fraction);
    }
    else
    {
        if(!Fraction) return Math.ceil(Value);
        if(Fraction > 0) return Math.ceil(Value*Math.pow(10,Fraction))/Math.pow(10,Fraction);
        if(Fraction < 0) return Math.ceil(Value/Math.pow(10,Fraction))*Math.pow(10,Fraction);
    }
};
/*Number*/
Number.Format = function(Format, Num)
{
    if(!Format)
        return Num.toString();
    var r;
    /*var r = /(r\d{1,2})/i;
    if(r.test(Format))
        return Format.replace(r, Num.toString(RegExp.$1.replace(/r/gi,"").ToInteger()));
    */
    r = Format.indexOf(";");
    var F = r >= 0?(Num >= 0 ? Format.Left(r) : Format.Right(Format.length - r -1)):(Num >= 0 ? Format : "-" + Format);
    if(!F)
        return Num.toString();
    var t = F.indexOf(",") >= 0; //thousand separator
    if(t)
        F = F.replace(/,/g,"");        
    var num = Math.abs(Num), numI="", numF="", numE="", firstP = -1, posE= -1, posD = -1, i, j, k, c, e = 0;
    num = num.toString();
    numI = num.match(/\d+\.{0,1}/)[0].match(/\d/g);
    if(!numI) numI=new Array();
    numF = num.match(/\.\d+/);
    if(numF)
        numF = numF[0].match(/\d/g);
    if(!numF) numF=new Array();
    F = F.match(/r\d{1,2}|e0+|e\+0+|e-0+|\.|./gi);
    r = /(r\d{1,2})/i;
    var r1 = /e0+|e\+0+|e-0+/i;
    for(i=0;i<F.length;i++)
    {
        if(F[i]=="."&&posD == -1)
            posD = i;
        if(posE== -1&&r1.test(F[i]))
            posE = i;
        if(r.test(F[i]))
            F[i] = F[i].replace(r, Num.toString(RegExp.$1.replace(/r/gi,"").ToInteger()));
    }
    r = /e0+|e\+0+|e-0+/i;
    j = posD==-1 ? F.length:posD;
    if(Num!=0&&posE>=0)
    {
        c = 0; e = 0;
        for(i=0;i<j;i++)
            if(F[i]=="0"||F[i]=="#") c++;
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
                        numI.push();
                    else
                        numI.push(numF.shift());
                    e--;
                }
                else
                    break;
            }
        }
        /*Format Exponent*/
        r = /e0+/i
        if(r.test(F[posE]))
            F[posE] = F[posE].Left(1)+e.Format(F[posE].Right(F[posE].length-1));
        else
        {
            r = F[posE].Right(F[posE].length-2);
            r="+"+r+";-"+r;
            F[posE] = F[posE].Left(1)+e.Format(r);
        }
    }
    k=-1;
    for(i=0; i<j; i++)
    {
        if(F[i]=="0"||F[i]=="#")
        {
            if(firstP == -1)
                firstP = i;
            if(k==-1 && F[i]=="0")
                k++;
            else if(k==0&&F[i]=="#")
                F[i] = "0";
        }
    }
    if(firstP==-1)
        firstP=posD;
    c=0;k=-1;
    for(i=j; i<F.length; i++)
        if(F[i]=="0"||F[i]=="#") c++;
    if(c < numF.length)
    {
        if(numF[c]>="5")
        {
            num =parseFloat(numI.join("")+"."+numF.join("")).toFixed(c);
            num = num.toString();
            numI = num.match(/\d+\.{0,1}/)[0].match(/\d/g);
            if(!numI) numI=new Array();
            numF = num.match(/\.\d+/);
            if(numF)
                numF = numF[0].match(/\d/g);
            if(!numF) numF=new Array();
        }
    }
    c = 0;
    for(i=j;i>=0;i--)
    {
        if(F[i]=="0"||F[i]=="#")
        {
            k=i;
            if(numI.length>0)
                F[i]=numI.pop();
            else
                F[i]= F[i]=="#"? "":F[i];
            c++;
            if(c==3&&t)
            {
                c = 0;
                F.splice(i,0,",");
                posD = posD==-1 ? posD : posD+1;
            }
        }
    }
    if(numI.length>0&&firstP >= 0)
    {
        while(numI.length>0)
        {
            F.splice(firstP,0,numI.pop());
            posD = posD==-1 ? posD : posD+1;
            c++;
            if(c==3&&t)
            {
                c = 0;
                F.splice(firstP,0,",");
                posD = posD==-1 ? posD : posD+1;
            }
        }
    }
    var lastP = -1;
    for(i=posD; i<F.length; i++)
    {
        if(i==-1) break;
        if(F[i]=="0"||F[i]=="#")
        {
            lastP = i;
            if(numF.length>0)
                F[i] = numF.shift();
            else
                F[i] = "0";
        }
    }       
    return F.join("");
};
(function()
{
    var obj = Number.prototype;
    obj.Format = function(Format)
    {
        return Number.Format.call(this, Format, this);
    };
})();
/*String*/
String.FormatString = function(args)//arg[0] is format
{
    if(!args) return "";
    if(args.length==0) return "";
    var r = args[0].match(/\}\}|\{\{|\{\d+\}|\{\d+:[n,b,s,d][^\}]+\}|./gi);
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
            r[i] = args[(r[i].toString().replace(/\{|\}/g,"").ToInteger()+1)];
            continue;
        }
        if((/^\{\d+:[n,b,s,d].+\}$/gi).test(r[i]))
        {
            r1 = r[i].match(/^\{\d+:|\}$|./g);
            r1[0] = r1[0].Right(r1[0].length-1).ToInteger();
            r1[1] = r1[1].Right(1).toUpperCase();
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
                    r[i] = (new Number(args[r1[0]+1])).Format(fmt);
                    break;
                case "B":
                    r[i] = (new Boolean(args[r1[0]+1])).Format(fmt);
                    break;
                case "S":
                    r[i] = args[r1[0]+1].toString();
                    break;
                case "D":
                    r[i] = (new Date(args[r1[0]+1].valueOf())).Format(fmt);
            }
        }
    }
    return r.join("");
};
String.Format = function()//String.Format(Format,....)
{
    return String.FormatString.call(this, arguments);
};
(function()
{
    var obj = String.prototype;
    obj.RTrim = function()
    {
        return this.replace(/\s+$/g,"");
    };
    obj.LTrim = function()
    {
        return this.replace(/^\s+/g,"");
    };
    obj.Trim = function()
    {
        return this.replace(/^\s+|\s+$/g,"");
    }
    obj.Left = function(Len)
    {
        return this.substring(0,Len);
    };
    obj.Right = function(Len)
    {
        return this.substring(this.length - Len, this.length );
    };
    obj.Replicate = function(N, Separator)
    {
        var ret = new Array();
        if(!Separator) Separator = "";
        for(var i = 0; i < N; i++)
            ret[i] = this;
        return ret.join(Separator);
    };
    obj.Copy = function(Position, Count)
    {
        if(!Position)
            Position = 0;
        if(!Count)
            Count = 1;
        return this.substr(Position, Count);
    };
    obj.ToDecimal = function(FractionDigits)
    {
        if(!FractionDigits)
            return parseFloat(this);
        return Math.Round(parseFloat(this),FractionDigits);
    };
    obj.IsDecimal = function()
    {
        return !(isNaN(parseFloat(this)));
    };
    obj.ToInteger = function(Radix)
    {
        return parseInt(this, Radix);
    };
    obj.IsInteger = function(Radix)
    {
        return !(isNaN(parseInt(this,Radix)));
    };
    obj.isNumber = function()
    {
        if(this.IsInteger()) return true;
        if(this.IsDecimal()) return true;
        return false;
    };
    obj.ToDateTime = function(Format)
    {
        if(!Format)
            return new Date(this);
        return Date.Parse(Format, this);
    };
    obj.Reverse = function()
    {
        var r = this.match(/.|^s/gi);
        if(!r) return "";
        return r.reverse().join("");
    }
})();    
/*Array*/    
Array.Contains = function(array, obj)
{
    for(var i in array)
        if(array[i]==obj) return true;
    return false;
};
(function()
{
    var obj = Array.prototype;
    obj.Contains = function(obj)
    {
        return Array.Contains(this, obj);
    }
})();
(function()
{
    var obj = Sys.Screen;
    obj.AvailableWidth = screen.availWidth;
    obj.AvailableHeight = screen.availHeight;
    obj.Width = screen.Width;
    obj.Height = screen.Height;
    obj.LastMouseScreenPoint = {X:0, Y:0};
    obj.LastMouseClientPoint = {X:0, Y:0};
})();