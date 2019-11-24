/*定义全局访问入口方法*/
$.extend({
    Pims: {
        Form: {},
        Ajax: {},
        Msg: {}
    }
});


/*异步异常处理*/
$(function () {
	// 设置jQuery Ajax全局的参数
	$.ajaxSetup({
        cache: false,
        //traditional: true,//布尔值，规定是否使用参数序列化的传统样式。这样后台数组变量才能接受到值
		error: function (jqXhr, textStatus, errorThrown) {

			if (jqXhr.responseJSON && jqXhr.responseJSON.Msg) {
				alert(jqXhr.responseJSON.Msg);
			} else {
				switch (jqXhr.status) {
					case (500):
						alert("服务器系统内部错误.");
						break;
					case (401):
						alert("当前请求无效（未登录），请重新登录后再使用.");
						break;
					case (403):
						alert("无权限执行此操作.");
						break;
					case (408):
						alert("当前请求已经超时.");
						break;
					default:
						alert("未知错误.");
				}
			}


            $.messager.alert('错误', "系统错误", 'error');
		}
	});
});


/*表单Form*/
(function (form) {
	
	form.Submit = function (jqSelector, url, callback) {
		var data = $(jqSelector).serialize();
		$.post(url, data, callback);
    }   

	//用jquery.form.js来作表单提交（方便文件上传）
	form.FileSubmit = function (jqSelector, url, callback) {

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
			success: callback

		}

		$(jqSelector).ajaxSubmit(options);
	}

	form.FormObject = function (jqSelector) {
		var formObj = {};
		var members = $(jqSelector).serializeArray();
		$.each(members, function (i, obj) {
			formObj[obj.name] = obj.value;
		});

		return formObj;
	}

	form.Clear = function (jqSelector) {
		$(jqSelector + ' :input').val("");
	}	

})($.Pims.Form);


(function (msg) {

	msg.Warning = function (content) {
		$.messager.alert('注意', content, 'warning');
	}

	msg.Error = function (content) {
		$.messager.alert('错误', content, 'error');
	}

	msg.Info = function (content) {
		$.messager.alert('提示', content, 'info');
	}	

	msg.Confirm = function (content, callback) {
		$.messager.confirm('确认',content,callback);
	}

	msg.Prompt = function (content, callback) {
		$.messager.prompt("输入", content, callback);
	}

})($.Pims.Msg);

(function (ajax) {

    ajax.Common = function (url,method, data, successCallback, errorCallback) {

        if (!successCallback)
            successCallback = function (r) {
                $.messager.show({ title: "消息", msg: '执行成功！', showType: 'show' });
            };

        if (!errorCallback)
            errorCallback = function (r) {
                $.messager.alert('错误', "执行失败！", 'error');
            };

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
            dataType: "json",
            contentType: "application/json;charset=utf-8",
            success: successCallback,
            error: errorCallback
        });
    }

    ajax.Post = function (url, data,successCallback,errorCallback) {
        $.Pims.Ajax.Common(url, 'post', data, successCallback, errorCallback);
    }

    ajax.Get = function (url, data, successCallback, errorCallback) {
        $.Pims.Ajax.Common(url, 'get', data, successCallback, errorCallback);
    }

    ajax.Put = function (url, data, successCallback, errorCallback) {
        $.Pims.Ajax.Common(url, 'put', data, successCallback, errorCallback);
    }

    ajax.Delete = function (url, data, successCallback, errorCallback) {
        $.Pims.Ajax.Common(url, 'delete', data, successCallback, errorCallback);
    }

    ajax.Response = function (r,successMsg) {
        if (r.success) {
            $.messager.alert('信息', successMsg, 'info');           

        }
        else if (r.unAuthorizedRequest) {
            window.location.href = r.targetUrl;
        }
        else {
            $.messager.alert('错误', r.error.message, 'error');
        }
    }

})($.Pims.Ajax);