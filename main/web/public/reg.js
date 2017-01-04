$(document).ready(function () {
    wx.config(
                { "debug": false, "beta": false, "appId": "wx7215c9cf19416f05", "nonceStr": "rand_585b1825f059e", "timestamp": 1482364965, "url": "https:\/\/www.xiongmaojinfu.com\/auth\/h5Register?mobile=&qsrc=xm-001-0430", "signature": "32ae1344dc43c59adf44751d62396c2fa9a7a8e8", "jsApiList": ["onMenuShareTimeline", "onMenuShareAppMessage", "onMenuShareQQ", "onMenuShareWeibo", "onMenuShareQZone"] });

    wx.ready(function () {

        wx.checkJsApi({
            jsApiList: [
                    'getNetworkType',
                    'previewImage',
                    'getLocation',
                    'onMenuShareTimeline',
                    'onMenuShareAppMessage'
                ],
            success: function (res) {
                console.log(res);
            }
        });
        wx.getLocation({
            type: 'wgs84', // 默认为wgs84的gps坐标，如果要返回直接给openLocation用的火星坐标，可传入'gcj02'
            success: function (res) {
                var latitude = res.latitude; // 纬度，浮点数，范围为90 ~ -90
                var longitude = res.longitude; // 经度，浮点数，范围为180 ~ -180。
                var speed = res.speed; // 速度，以米/每秒计
                var accuracy = res.accuracy; // 位置精度
                var latitudeval = '<input type="hidden" id="latitude" value=" ' + latitude + ' ">';
                var longitudeval = '<input type="hidden" id="longitude" value=" ' + longitude + ' ">';
                $("#lashow").html(latitudeval);
                $("#loshow").html(longitudeval);
                //                    alert(latitude + "-" + latitude);
                //                    window.location.href = "http://test.xiongmaojinfu.com/test/getLocation?latitude=" + latitude + "&longitude=" + longitude;

            },
            cancel: function (res) {

            }
        });
    });
});


var url = "https://www.xiongmaojinfu.com";  
var mobile = "18310807769";
var qsrc = "xm-001-0384";
//验证注册
var phone = '', msgCode = '', password = '', lashow = '', loshow = '';

$('#getCode').on('click', getCodeClickEvt);
$("#checkForm").on('click', registerClickEvt);

function getCodeClickEvt() {
    phone = $("#phone").val(); 
    if (phone == '' || phone.length < '11' || !/^1[3|4|5|7|8][0-9]\d{8}$/.test(phone)) {
        alert('请输入正确的手机号！');
        $("#phone").focus();
    }
    else {
        var timmer = null,
                    interval = 60,
                    $this = $(this),
                    phone = $.trim($('#phone').val());
        $.ajax({
            type: "POST",
            url: "../../Reg.ashx",
            data: {
                'action': 'sendCode',
                'mobile': phone
            },
            dataType: "json",
            success: function (res) {

                if (res.error == true) {
                    alert(res.msg[0]);
                    return false;
                }
                $('#popreg').show();
                _auto();
            }
        }); 
        function _auto() {
            $this.addClass('disabled').html('<div style="color:#a0a0a0;margin-left:20%;">重发(' + interval + '秒)</div>');
            $this.prop('disabled', true);

            timmer = setTimeout(function () {
                if (interval > 0) {
                    interval--;
                    $this.html('<div style="color:#a0a0a0;margin-left:20%;">重发(' + interval + '秒)</div>');
                    setTimeout(arguments.callee, 1000);
                } else {
                    clearTimeout(timmer);
                    $this.removeClass('disabled').html('<div style="color:#a0a0a0;margin-left:20%;">重发</div>');
                    $this.prop('disabled', false);
                }
            }, 1000)
        }
    }
}

function registerClickEvt(){ 
    phone = $("#phone").val();
    msgCode = $("#msgCode").val();
    password = $("#password").val();
    lashow = $("#lashow").val();
    loshow = $("#loshow").val();

    if (phone === '' || phone.length < '11' || !/^1[3|4|5|7|8][0-9]\d{8}$/.test(phone)) {
        alert('请输入正确的手机号!');
        return false;
    } else if (msgCode === '') {
        alert('请输入手机验证码!');
        return false;
    } else if (password === '' || password.length < '6') {
        alert('请输入密码且不能少于6位!');
        return false;
    }

    //表单提交无误开始进行注册行为 

    $.ajax({
        type: "POST",
        url: "../../Reg.ashx",
        data: {
            'action':'register',
            'mobile': phone,
            'pwd': password,
            'code_key': msgCode,
            'inkey': mobile, 
            'channel_no': qsrc,
            'lashow': lashow,
            'loshow': loshow
        },
        dataType: "json",
        success: function (msg) { 
            if (msg.error) {
                alert(msg.msg[0]);
            } else {
                window.location.href = url + "/auth/h5RegisterSuccess/" + phone + "/" + qsrc;
            }
        }
    });
}