// JavaScript Document
var enroll=document.getElementById("enroll");//得到enroll文件赋值给定义变量enroll
enroll.onclick = function(e)
{
  e.preventDefault();//防止跳转
  enroll.innerHTML = "参加成功";//内部显示
  enroll.style.background = "#6F6";//背景颜色
  enroll.style.color="#FFF";//字体颜色
}