function loginValidator()
{
    $(".ui.form").form({
        fields: {
            account: {
                identifier: 'MainContent_Account_Input',
                rules: [
                    {
                        type: 'empty',
                        prompt: '請輸入帳號'
                    },
                    {
                        type: 'regExp',
                        value: /^[\dA-Za-z]*$/i,
                        prompt: '請不要輸入非英文數字之文字'
                    }
                ]
            },
            password: {
                identifier: 'MainContent_Password_Input',
                rules: [
                    {
                        type: 'empty',
                        prompt: '請輸入密碼'
                    },
                    {
                        type: 'regExp',
                        value: /^[\dA-Za-z]*$/i,
                        prompt: '請不要輸入非英文數字之文字'
                    }
                ]
            }
        },
        onSuccess: function () {
            console.log("success");
            return true;
        }
    });
    return false;
}

$(document).ready(function ()
{

});