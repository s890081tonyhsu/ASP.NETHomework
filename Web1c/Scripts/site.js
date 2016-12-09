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

function load_drink_names()
{
    if($('#MainContent_OrderDrinks_GridView_OrderDrink_Template_drink_id_dropdown_0').length == 0) return;
    appendStr = '';
    $('#MainContent_OrderDrinks_GridView_OrderDrink_Template_drink_id_dropdown_0').children('option').each(function(){
        appendStr = appendStr + '<div class="item" data-value="'+$(this).val()+'">'+$(this).text()+'</div>';
    });
    $('#OrderDrink_Template_drink_id_dropdown').append(appendStr);
}

$(document).ready(function ()
{
    load_drink_names();
    $(".ui.dropdown").dropdown();
});