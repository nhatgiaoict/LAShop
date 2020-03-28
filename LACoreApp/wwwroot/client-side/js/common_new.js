function ConfirmDelete(mess)
{
	if(window.confirm(mess)==true)
	{
		return true;
	}
	return false;
}
function ShowTR(cbxClick, TRShow) {
	var cbx = document.getElementById(cbxClick);
	if (cbx != null) {
		if (cbx.checked && cbx.disabled==false) TRShow.className = "on";
		else TRShow.className = "off";	
	}
	else
		alert("Error!");
}
function LoadTR(formName, cbxClick, TRShow) {
	var cbx = false;
	eval("var theform = document." + formName + ";");
	var len = theform.elements.length;
	for (var j = 0; j < len; j++) {
		var e = theform.elements[j];
		if (e.id.indexOf(cbxClick)>=0) {
			if(e.checked == true && e.disabled == false){
			cbx = true;break;
			}
		}
	}
	if (cbx) 
		TRShow.className = "on";
	else 
		TRShow.className = "off";	
}
function ShowTD(cbxClick, TDShow, TDHide) {
	var cbx = document.getElementById(cbxClick);
	if (cbx != null) {
		if (cbx.checked && cbx.disabled==false) {
			TDShow.className = "on";
			TDHide.className = "off";
		}
		else {
			TDShow.className = "off";
			TDHide.className = "on";	
		}
	}
	else
		alert("Error!");
}
function LoadTD(formName, cbxClick, TDShow, TDHide) {
	var cbx = false;
	eval("var theform = document." + formName + ";");
	var len = theform.elements.length;
	for (var j = 0; j < len; j++) {
		var e = theform.elements[j];
		if (e.id.indexOf(cbxClick)>=0) {
			if(e.checked == true && e.disabled == false){
			cbx = true;break;
			}
		}
	}
	if (cbx){
		TDShow.className = "on";
		TDHide.className = "off";
	}
	else {
		TDShow.className = "off";
		TDHide.className = "on";	
	}
}
function CheckDisabled(formName, checkName, Disabled) {
	eval("var theform = document." + formName + ";");
	var len = theform.elements.length;
	for (var j = 0; j < len; j++) {	
		var e = theform.elements[j];
		if (e.name.indexOf(checkName)>=0) {
			if(Disabled=="true"){
				e.disabled = true;
				//e.checked = false; 
				}
			else e.disabled = false;
		}
	}
}
function embed_flash(swf_card, swf_width, swf_height, id){
	var str = '<OBJECT classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=5,0,0,0" ';
		if (swf_width != '') 
			str += 'WIDTH='+swf_width+' ';
		if(swf_height != '')
			str += 'HEIGHT='+swf_height+' ';
		str += 'id="'+id+'">';
		str += '<PARAM NAME=movie VALUE="'+swf_card+'">';
		str += '<PARAM NAME=quality VALUE=high>';
		str += '<PARAM NAME=wmode VALUE="Transparent">';
		str += '<PARAM NAME=bgcolor VALUE="">';
		str += '<PARAM NAME=menu VALUE=false>';
		str += '<EMBED style="CURSOR: hand" src="'+swf_card+'" quality=high ';
		if (swf_width != '') 
			str += 'WIDTH='+swf_width+' ';
		if(swf_height != '')
			str += 'HEIGHT='+swf_height+' ';
		str += 'bgcolor="" menu=false wmode=Transparent TYPE="application/x-shockwave-flash" PLUGINSPAGE="http://www.macromedia.com/shockwave/download/index.cgi?P1_Prod_Version=ShockwaveFlash">';
		str += '</embed>';
		str += '</object>';
	document.write(str);
}

function embed_music(midi_file){
	document.write('<EMBED SRC='+midi_file+' WIDTH=120 HEIGHT=60 AUTOSTART=true LOOP=true HIDDEN=false>');
}
function Calendar_vn(){
	calendar = new Date();
	day = calendar.getDay();
	month = calendar.getMonth();
	date = calendar.getDate();
	year = calendar.getYear();
	if (year< 1000) year = 1900 + year;
	cent = parseInt(year/100);
	g = year % 19;
	k = parseInt((cent - 17)/25);
	i = (cent - parseInt(cent/4) - parseInt((cent - k)/3) + 19*g + 15) % 30;
	i = i - parseInt(i/28)*(1 - parseInt(i/28)*parseInt(29/(i+1))*parseInt((21-g)/11));
	j = (year + parseInt(year/4) + i + 2 - cent + parseInt(cent/4)) % 7;
	l = i - j;
	emonth = 3 + parseInt((l + 40)/44);
	edate = l + 28 - 31*parseInt((emonth/4));
	emonth--;
	var dayname = new Array ("Chủ nhật", "Thứ hai", "Thứ ba", "Thứ tư", "Thứ năm", "Thứ sáu", "Thứ bảy" );
	var monthname = 
	new Array ("01","02","03","04","05","06","07","08","09","10","11","12" );
	var datename = 
	new Array ("0","01","02","03","04","05","06","07","08","09","10","11","12","13","14","15","16","17","18","19","20","21","22","23","24","25","26","27","28","29","30","31" );
	document.write(dayname[day]+", ");
	document.write(datename[date] + " - ");
	document.write(monthname[month]+" - ");
	document.write(""+year +"");
}
function Calendar_en(){
	calendar = new Date();
	day = calendar.getDay();
	month = calendar.getMonth();
	date = calendar.getDate();
	year = calendar.getYear();
	if (year< 1000) year = 1900 + year;
	cent = parseInt(year/100);
	g = year % 19;
	k = parseInt((cent - 17)/25);
	i = (cent - parseInt(cent/4) - parseInt((cent - k)/3) + 19*g + 15) % 30;
	i = i - parseInt(i/28)*(1 - parseInt(i/28)*parseInt(29/(i+1))*parseInt((21-g)/11));
	j = (year + parseInt(year/4) + i + 2 - cent + parseInt(cent/4)) % 7;
	l = i - j;
	emonth = 3 + parseInt((l + 40)/44);
	edate = l + 28 - 31*parseInt((emonth/4));
	emonth--;
	var dayname = new Array ("Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" );
	var monthname = 
	new Array ("January","February","March","April","May","June","July","August","September","October","November","December" );
	var datename = 
	new Array ("0","01","02","03","04","05","06","07","08","09","10","11","12","13","14","15","16","17","18","19","20","21","22","23","24","25","26","27","28","29","30","31" );
	document.write(dayname[day]+", ");
	document.write(monthname[month]+" ");
	document.write(datename[date] + ", ");
	document.write(""+year +"");
}
function Calendar(lang){
	if(lang=="vn")
	Calendar_vn();
	else
	Calendar_en();	
}
function ShowPopup(in_url,in_width,in_height)
{
	winDef = 'status=no,resizable=yes,scrollbars=yes,toolbar=no,location=no,fullscreen=no,titlebar=yes,height='.concat(in_height).concat(',').concat('width=').concat(in_width).concat(',');
	winDef = winDef.concat('top=').concat((screen.height - in_height)/2).concat(',');
	winDef = winDef.concat('left=').concat((screen.width - in_width)/2);
	newwin = window.open(in_url, 'image', winDef);
}
function SelectFile(txtChoice)
{
	ShowPopup('../filemanager/default.aspx?txtChoice='+txtChoice,580,400)

}
function SelectFileVideo(txtChoiceVideo)
{
	ShowPopup('../filemanagerVideo/default.aspx?txtChoice='+txtChoiceVideo,580,400)
}
function CheckLen(Target, num)
{
	
	StrLen = Target.value.length;
	if (StrLen > num )
	{
		Target.value = Target.value.substring(0,num);
		charsLeft = 0;
	}
	else
	{
		charsLeft = num - StrLen;
	}
}
function ResetForm(FormName)
{
	FormName.reset();
	return false;
}

function ShowHideTab(controlID) {
    if (controlID == 'TSKT') {
        $("#TSKT").show();

        $("#headKT").removeClass().addClass("tabs--tab tabs--selected-tab");
        
        $("#TlbtA").hide();

        $("#headELKT").removeClass().addClass("tabs--tab");
    }
    if (controlID == 'TSKT') {

        $("#TlbtA").hide();

        $("#headKT").removeClass().addClass("tabs--tab");

        $("#TSKT").show();

        $("#headELKT").removeClass().addClass("tabs--tab tabs--selected-tab");
    }
}