// JavaScript Document
$(document).ready//文件.准备
(
	function a()//函数
	{
		var test = "欢迎来到刺客首页";
		//alert(test);
	}
)

	var nav=document.getElementById("nav");	 
	nav.style.display="none"; 
	document.getElementById("navdisplay").onclick=function sk()
	{ 
		if(nav.style.display=="none")
		{	 
		   nav.style.display="block";			
		}
		else
		{
			 nav.style.display="none";
		}
    }
