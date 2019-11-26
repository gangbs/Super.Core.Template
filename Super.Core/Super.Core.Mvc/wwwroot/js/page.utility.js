/*定义全局访问入口方法*/
$.extend({
    utility: {
        form: {},
        ajax: {},
        msg: {}
    }
});

(function (ajax) {

    ajax.common = function (url, method, data, successCallback, errorCallback) {

        if (!successCallback)
            successCallback = $.utility.ajax.successHandler;

        if (!errorCallback)
            errorCallback = $.utility.ajax.errorHandler;

        if (method == "get") {
            data = {};
            var queryString = "";
            if (data != undefined || data != null) {
                $.each(data, function (k, v) {
                    if (queryString != "")
                        queryString += "&";
                    queryString += k + "=" + v
                });
            };

            if (url.endsWith("?") || url.endsWith("&"))
                url = url + queryString;
            else
                url = url + "?" + queryString;
        }

        $.ajax({
            url: url,
            type: method,
            data: JSON.stringify(data),
            //dataType: "json",//若没有指定则根据响应中的Content-Type头来处理
            contentType: "application/json;charset=utf-8",
            success: successCallback,
            error: errorCallback
        });
    }

    ajax.post = function (url, data, successCallback, errorCallback) {
        $.utility.ajax.common(url, 'post', data, successCallback, errorCallback);
    }

    ajax.get = function (url, data, successCallback, errorCallback) {
        $.utility.ajax.common(url, 'get', data, successCallback, errorCallback);
    }

    ajax.put = function (url, data, successCallback, errorCallback) {
        $.utility.ajax.common(url, 'put', data, successCallback, errorCallback);
    }

    ajax.delete = function (url, data, successCallback, errorCallback) {
        $.utility.ajax.common(url, 'delete', data, successCallback, errorCallback);
    }

    ajax.successHandler = function (r) {
        if (r && r.message)
            $.utility.msg.success(r.message);
        else
            $.utility.msg.success("执行成功！");
    }

    ajax.errorHandler = function (jqXhr, textStatus, errorThrown) {
        if (jqXhr.responseJSON && jqXhr.responseJSON.message) {
            $.utility.msg.fail(jqXhr.responseJSON.message);
        }
        else {
            switch (jqXhr.status) {
                case (500):
                    $.utility.msg.fail("服务器系统内部错误");
                    break;
                case (401):
                    $.utility.msg.fail("当前请求无效（未登录），请重新登录后再使用");
                    break;
                case (403):
                    $.utility.msg.fail("无权限访问该资源");
                    break;
                case (404):
                    $.utility.msg.fail("所访问资源不存在");
                    break;
                case (408):
                    $.utility.msg.fail("当前请求已经超时");
                    break;
                default:
                    $.utility.msg.fail("未知系统错误,code:" + jqXhr.status);
                    break;
            }
        }
    }

})($.utility.ajax);

/*表单Form*/
(function (form) {

    form.submit = function (jqSelector, url, successCallback, errorCallback) {
        var data = $(jqSelector).serialize();
        $.utility.ajax.post(url, data, successCallback, errorCallback);
    }

    //用jquery.form.js来作表单提交（方便文件上传）
    form.fileSubmit = function (jqSelector, url, successCallback, errorCallback) {

        var options = {
            url: url,
            type: 'post',
            dataType: 'json',//Expected data type of the response
            //data: data,//An object containing extra data that should be submitted along with the form.
            clearForm: false,//Boolean flag indicating whether the form should be cleared if the submit is successful
            resetForm: false,//Boolean flag indicating whether the form should be reset if the submit is successful
            beforeSerialize: function ($form, options) {
                // return false to cancel submit                  
            },
            beforeSubmit: function (arr, $form, options) {
                // The array of form data takes the following form: 
                // [ { name: 'username', value: 'jresig' }, { name: 'password', value: 'secret' } ] 

                // return false to cancel submit                  
            },
            success: successCallback,
            error: errorCallback

        }

        $(jqSelector).ajaxSubmit(options);
    }

    form.formObject = function (jqSelector) {
        var formObj = {};
        var members = $(jqSelector).serializeArray();
        $.each(members, function (i, obj) {
            formObj[obj.name] = obj.value;
        });

        return formObj;
    }

    form.clear = function (jqSelector) {
        $(jqSelector + ' :input').val("");
    }

})($.utility.form);

/*通用消息*/
(function (msg) {

    msg.success = function (content) {
        $.messager.alert('成功', content, 'info');
    }

    msg.fail = function (content) {
        $.messager.alert('失败', content, 'error');
    }

    msg.warn = function (content) {
        $.messager.alert('注意', content, 'warning');
    }

    msg.error = function (content) {
        $.messager.alert('错误', content, 'error');
    }

    msg.info = function (content) {
        $.messager.alert('信息', content, 'info');
    }

    msg.confirm = function (content, callback) {
        $.messager.confirm('确认', content, callback);
    }

    msg.prompt = function (content, callback) {
        $.messager.prompt("输入", content, callback);
    }

})($.utility.msg);

/*异步异常处理*/
$(function () {
	// 设置jQuery Ajax全局的参数
	$.ajaxSetup({
        cache: false,
        //traditional: true,//布尔值，规定是否使用参数序列化的传统样式。这样后台数组变量才能接受到值
		error: function (jqXhr, textStatus, errorThrown) {
            $.utility.ajax.errorHandler(jqXhr, textStatus, errorThrown);          
		}
	});
});



