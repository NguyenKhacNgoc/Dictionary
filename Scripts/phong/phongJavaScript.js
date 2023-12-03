

var id_text_search = [];
var user_info = [];
var json_cookie = getCookie('search_his');
if (json_cookie != null && json_cookie.trim() !== "") {
    var json_cookie_ar = JSON.parse(json_cookie);
    id_text_search = json_cookie_ar;
}

var json_user = getCookie('LoggedUser');
var id_user = null;
if (json_user != null && json_user.trim() !== "") {
    var json_cookie_ar = JSON.parse(json_user);
    user_info = json_cookie_ar;
    id_user = user_info.id;
   
} else {
    search_word_history_cookie();
}

document.getElementById('p_searched').addEventListener('click', function (e) {
    var p_searchedWrap = document.querySelector('.search_history_wrap');
    p_searchedWrap.classList.add('p_active');
    black_cover();
    

    if (id_user == null) {
        search_word_history_cookie();
    } else {
        get_search_word_history();
    }
});

document.getElementById('search-input').addEventListener('input', function (e) {
    search_word();

});

document.addEventListener('click', function (event) {
    var listSearch = document.getElementById('list_search');
    var pSearchWrap = document.querySelector('.search-box');
    var searchInput = document.getElementById('search-input');
    var wordItem = event.target.closest('.word_item');
    var search_in_other = document.querySelector('.search_in_other');
    var p_close = document.querySelector('.p_popup.p_active .p_close');
    var black_cover_click = document.querySelector('.black_cover');
    var p_popup_active = document.querySelector('.p_popup.p_active');

    if (wordItem !== null && wordItem.contains(event.target) && !wordItem.classList.contains('p_result')) {
        var remove_his = wordItem.querySelector('.p_remove');
        if (event.target == remove_his) {
            remove_from_histoer_search(wordItem.getAttribute("data-id-word"));
        } else {
            if (listSearch.classList.contains('p_active')) {
                listSearch.classList.remove('p_active');
            }
            search_word_detail(wordItem.getAttribute("data-id-word"));
        }

        
       
    }



    if (pSearchWrap !== null && !pSearchWrap.contains(event.target)) {
        if (listSearch.classList.contains('p_active')) {
            listSearch.classList.remove('p_active');
        }

    }

    if (event.target == searchInput && searchInput.value !== '') {
        listSearch.classList.add('p_active');
    }

    if (event.target == p_close ) {
        if (p_popup_active.classList.contains('p_active')) {
            p_popup_active.classList.remove('p_active');
            black_cover();
        }
    }

    if (event.target == black_cover_click) {
        if (p_popup_active.classList.contains('p_active')) {
            p_popup_active.classList.remove('p_active');
            black_cover();
        }
    }

    if (event.target == search_in_other) {
        var search_in_other_farme_wrap = document.querySelector('.extra_search');
        var search_in_other_farme = document.querySelector('.extra_search #lbdict_frame_view');

        if (search_in_other_farme != null) {
            search_in_other_farme_wrap.classList.add('p_active');
            black_cover();
            if (listSearch.classList.contains('p_active')) {
                listSearch.classList.remove('p_active');
            }
        }
     }


});



document.querySelector('.p_select_lang, .p_select_lang_trans').addEventListener('change', function () {
    search_word()
});

function remove_from_histoer_search(id) {
    var search_history_wrap = document.querySelector('.search_history_wrap ');
    if (id_user == null) {
        for (let i = 0; i < id_text_search.length; i++) {
            if (id_text_search[i].id === id) {
                id_text_search.splice(i, 1);
            }
        }
        console.log(id_text_search);
        var t = JSON.stringify(id_text_search);

        document.cookie = 'search_his=' + t + ';expires=' + new Date + '';
        search_word_history_cookie();
    } else {
        var xhr = new XMLHttpRequest();
        var url = '/Home/Remove_from_history_search';

        xhr.open('GET', url + '?id_user=' + encodeURIComponent(id_user) + '&id_word=' + encodeURIComponent(id), true);

        xhr.onreadystatechange = function () {
            if (xhr.readyState === 4 && xhr.status === 200) {
                console.log(xhr.responseText);
                get_search_word_history();
            }
        };

        xhr.send();
        
    }

    setTimeout(function () {
        var search_history_wrap = document.querySelector('.search_history_wrap ');
        var search_history_div = search_history_wrap.querySelector('.search_history');
        var item = search_history_div.querySelector('.word_item ');
        if (item == null && search_history_wrap.classList.contains('p_active')) {
            black_cover();
            search_history_wrap.classList.remove('p_active')
        }
    }, 400);

}

function black_cover() {
    var black_cover = document.querySelector('.black_cover');
    if (black_cover.classList.contains('p_active')) {
        black_cover.classList.remove('p_active');
    } else {
        black_cover.classList.add('p_active');
    }
}

function search_word() {
    var searchInputValue = document.getElementById('search-input').value;
    var selectedLang = document.querySelector('.p_select_lang option:checked').value;
    var selectedLangTrans = document.querySelector('.p_select_lang_trans option:checked').value;
    var listSearch = document.getElementById('list_search');

    if (searchInputValue != '') {
        listSearch.classList.add('p_active');
        var xhr = new XMLHttpRequest();
        var url = '/Home/Search_result';

        xhr.open('GET', url + '?text=' + encodeURIComponent(searchInputValue) + '&lang=' + encodeURIComponent(selectedLang) + '&lang_tran=' + encodeURIComponent(selectedLangTrans), true);

        xhr.onreadystatechange = function () {
            if (xhr.readyState === 4 && xhr.status === 200) {
                document.getElementById('list_search').innerHTML = xhr.responseText;
            }
        };

        xhr.send();
    } else {

        var pDropDownElement = listSearch.querySelector('.p_drop_down');

        if (pDropDownElement !== null) {
            // Tạo chuỗi HTML mới cho phần tử <a>
            var newAnchorHTML = '<a href="javascript:void(0)" class="p_empty  p_flex_start">Bạn đang bỏ trống</a>';

            // Thay thế nội dung HTML của phần tử .p_drop_down
            pDropDownElement.innerHTML = newAnchorHTML;
            listSearch.classList.remove('p_active');
        } else {

            console.log('Không có phần tử con trong #list_search');
        }
    }
}


function search_word_detail(id_search) {
    var result = document.getElementById('result');
    
    var d = new Date,
        dformat = [d.getMonth() + 1,
        d.getDate(),
        d.getFullYear()].join('/') + ' ' +
            [d.getHours(),
            d.getMinutes(),
            d.getSeconds()].join(':');

    const search_his = {
        id: id_search,
        date_time: dformat
    };

    let isIdExist = false;


    for (let i = 0; i < id_text_search.length; i++) {
        if (id_text_search[i].id === id_search) {

            id_text_search[i].date_time = dformat;
            isIdExist = true;
            break;
        }
    }

    if (!isIdExist) {
        id_text_search.push(search_his);
    }

    id_text_search.sort((a, b) => {

        const dateA = new Date(a.date_time);
        const dateB = new Date(b.date_time);

        return dateB - dateA;
    });
    var t = JSON.stringify(id_text_search);

    document.cookie = 'search_his=' + t + ';expires=' + dformat + '';

    if (id_search != '') {
        var xhr = new XMLHttpRequest();
        var url = '/Home/Search_result_detail';

        xhr.open('GET', url + '?id=' + encodeURIComponent(id_search), true);

        xhr.onreadystatechange = function () {
            if (xhr.readyState === 4 && xhr.status === 200) {
                result.innerHTML = xhr.responseText;

                if (id_user == null) {
                    search_word_history_cookie();
                } else {
                    search_word_history(id_search,dformat);
                }
            }
        };

        xhr.send();
    }
}

function search_word_history(id_word, datetime) {
    var search_history_div = document.querySelector('.search_history');
    var xhr = new XMLHttpRequest();
    var url = '/Home/Add_history';
    xhr.open('GET', url + '?id_user=' + encodeURIComponent(id_user) + '&id_word=' + encodeURIComponent(id_word) + '&datetime=' + encodeURIComponent(datetime), true);

    xhr.onreadystatechange = function () {
        if (xhr.readyState === 4 && xhr.status === 200) {
            search_history_div.innerHTML = xhr.responseText;
        }
    };

    xhr.send();
}

function get_search_word_history() {
    var search_history_wrap = document.querySelector('.search_history_wrap ');
    var search_history_div = search_history_wrap.querySelector('.search_history');
    var xhr = new XMLHttpRequest();
    var url = '/Home/History_return';
    xhr.open('GET', url + '?id_user=' + encodeURIComponent(id_user), true);

    xhr.onreadystatechange = function () {
        if (xhr.readyState === 4 && xhr.status === 200) {
            search_history_div.innerHTML = xhr.responseText;
        }
    };

    xhr.send();
}

function search_word_history_cookie() {
    var search_history_wrap = document.querySelector('.search_history_wrap ');
    var search_history_div = search_history_wrap.querySelector('.search_history');
    var ids = [];

    if (id_text_search.length >= 0) {
        id_text_search.forEach(function (item) {
            var id = item.id;
            ids.push(id)
        });

        var jsonData = JSON.stringify(ids);
        var xhr = new XMLHttpRequest();
        var url = '/Home/Item_history';
        console.log(jsonData);
        xhr.open('GET', url + '?idList=' + encodeURIComponent(jsonData), true);

        xhr.onreadystatechange = function () {
            if (xhr.readyState === 4 && xhr.status === 200) {
                search_history_div.innerHTML = xhr.responseText;
                id_text_search.forEach(item => {
                    // Tìm phần tử DOM dựa trên data-id-word
                    var wordItem = search_history_div.querySelector(`.word_item[data-id-word="${item.id}"]`);

                    // Nếu tìm thấy, cập nhật text cho span.phongll
                    if (wordItem) {
                        var time_search = wordItem.querySelector('.time_search');
                        time_search.textContent = "Thời điểm tìm kiếm: " + item.date_time;
                    }
                });
            }
        };

        xhr.send();
    }
    
}

function getCookie(cname) {
    let name = cname + "=";
    let decodedCookie = decodeURIComponent(document.cookie);
    let ca = decodedCookie.split(';');
    for (let i = 0; i < ca.length; i++) {
        let c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}