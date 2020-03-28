
$(document).ready(function() {
    var nav = function() {
        $('.gw-nav > li > a').click(function() {
            var gw_nav = $('.gw-nav');
            gw_nav.find('li').removeClass('active');
            $('.gw-nav > li > ul > li').removeClass('active');

            var checkElement = $(this).parent();
            var ulDom = checkElement.find('.gw-submenu')[0];

            if (ulDom == undefined) {
                checkElement.addClass('active');
                $('.gw-nav').find('li').find('ul:visible').slideUp();
                return;
            }
            if (ulDom.style.display != 'block') {
                gw_nav.find('li').find('ul:visible').slideUp();
                gw_nav.find('li.init-arrow-up').removeClass('init-arrow-up').addClass('arrow-down');
                gw_nav.find('li.arrow-up').removeClass('arrow-up').addClass('arrow-down');
                checkElement.removeClass('init-arrow-down');
                checkElement.removeClass('arrow-down');
                checkElement.addClass('arrow-up');
                checkElement.addClass('active');
                checkElement.find('ul').slideDown(300);
            } else {
                checkElement.removeClass('init-arrow-up');
                checkElement.removeClass('arrow-up');
                checkElement.removeClass('active');
                checkElement.addClass('arrow-down');
                checkElement.find('ul').slideUp(300);

            }
        });
        $('.gw-nav > li > ul > li > a').click(function() {
            $(this).parent().parent().parent().removeClass('active');
            $('.gw-nav > li > ul > li').removeClass('active');
            $(this).parent().addClass('active')
        });
    };
    nav();

    autoResizeSlideHome();
});
/**/

function checknumber(numb, value){
var anum=/(^\d+$)|(^\d+\.\d+$)/
if (anum.test(numb))testresult=true
else{alert(value + " Bạn phải nhập số");testresult=false
}return (testresult)} 

function validRequireField(str, name) {
    if ($.trim(str) == "" || $.trim(str) == name) {
        alert(name + " không được để trống.");
        return false;
    }
    return true;
}
function validEmail(email) {
    var reg = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;
    if (reg.test(email) == false) {
        alert("Sai định dạng email.");
        return false;
    }
    return true;
}
function RefreshCaptchaLH(){
    var img = document.getElementById("imgCaptcha");
    img.src = "/control/CaptchaHandler.ashx?query=" + Math.random();
}
function GetVaulesCheckBox()
{
var Ckd = document.getElementById("chkSendcopy");
if(Ckd.checked) return 1;
else return 0;
}

var nameLh = '';
var telLh = '';
var emailLh = '';
var contentLh = '';
var captchaLh = '';
function GuiYeuCau() {
    if (validRequireField($("#txtHoten").val(), "Họ tên")
    && validEmail($("#txtEmail").val())
    && validRequireField($("#txtTel").val(), "Điện thoại")
    && validRequireField($("#txtNoidung").val(), "Nội dung")
     && validRequireField($("#txtCode").val(), "Mã xác nhận")
    ) {
        nameLh = $("#txtHoten").val();
        emailLh = $("#txtEmail").val();
        contentLh = $("#txtNoidung").val();
        telLh = $("#txtTel").val();
        captchaLh = $("#txtCode").val();
        var dataString = "namelh=" + nameLh + "&emaillh=" + emailLh + "&tellh=" + telLh + "&noidunglh=" + contentLh + "&code=" + captchaLh;
        $.ajax({
            type: "POST",
            url: "/control/Update_lh.ashx",
            data: dataString,
            cache: false,
            beforeSend: function() {
                $("#divloadding").show();
            },
            complete: function() {
                $("#divloadding").hide();
            },
            success: function(data) {
                //$("#divloadding").css('display', 'block');
                if (data == "1") { $("#divloadding").css('display', 'none');
                    alert("Mã xác nhận đã nhập không chính xác, Vui lòng thử lại.");
                    return;
                } else {
                    $("#divloadding").css('display', 'none');
                    alert("Thông tin của bạn đã được gửi tới chúng tôi.");
                }
            }
        });
    }
    return false;
}
function Resetsend() {
    $("#txtname").val('');
    $("#txtdiachi").val('');
    $("#txtemail").val('');
    $("#txtmobile").val('');
    $("#txttel").val('');
    $("#txtnoidung").val('');
}
function share_linkhay() { u = location.href;window.open("http://linkhay.com/login?return=/submit?link_url=" + encodeURIComponent(u)); }
function share_twitter() { u = location.href; t = document.title; window.open("http://twitter.com/home?status=" + encodeURIComponent(u)); }
function share_facebook() { u = location.href; t = document.title; window.open("http://www.facebook.com/share.php?u=" + encodeURIComponent(u) + "&t=" + encodeURIComponent(t)); }
function share_google() { u = location.href; t = document.title; window.open("http://www.google.com/bookmarks/mark?op=edit&bkmk=" + encodeURIComponent(u) + "&title=" + t + "&annotation=" + t); }
function share_buzz() { u = location.href; t = document.title; window.open("http://buzz.yahoo.com/buzz?publisherurn=tinquangcaop&targetUrl=" + encodeURIComponent(u)); }
function share_Zing() { u = location.href; t = document.title;window.open("http://link.apps.zing.vn/share?u=" + encodeURIComponent(u) + "&t=" + encodeURIComponent(t)); }

function share_LinhHay() { u = location.href; t = document.title;window.open(" http://linkhay.com/submit?url=" + encodeURIComponent(u) + "&t=" + encodeURIComponent(t)); }

function locdau(str){str= str.toLowerCase();  
      str= str.replace(/!|@|%|\^|\*|\(|\)|\+|\=|\<|\>|\?|\/|,|\.|\:|\;|\'| |\"|\&|\#|\[|\]|~|$|_/g,"-"); 
      str= str.replace(/-+-/g,"-"); //thay thế 2- thành 1- 
      str= str.replace(/^\-+|\-+$/g,""); return str;
  }

  function ShowHideTab(controlID) {
      if (controlID == 'tab1') {
          $("#Contenttap1").show();
          $("#tab2").removeClass().addClass("");
          $("#Contenttap2").hide();
          $("#tab1").removeClass().addClass("selected");
      }
      if (controlID == 'tab2') {
          $("#Contenttap2").show();
          $("#tab1").removeClass();
          $("#Contenttap1").hide();
          $("#tab2").removeClass().addClass("selected");
      }
  }
  function autoResizeSlideHome() {
      var hdHeight = 56;
      var lfWidth = 0;
      var winHeight = $(window).height();
      var winWidth = $(window).width();

      var newSlideHeight = winHeight - hdHeight;
      var newSlideWidth = winWidth - lfWidth;
      $('#myCarousel div.carousel-inner .item img').css({ "width": newSlideWidth + 'px' });
      $('#myCarousel div.carousel-inner .item img').css({ "max-height": newSlideHeight + 'px' });
      $('#myCarousel div.carousel-inner .item img').css({ "overflow": "hidden" });
      $('#myCarousel div.carousel-inner .item img').css({ "object-position": "center" });
  }
