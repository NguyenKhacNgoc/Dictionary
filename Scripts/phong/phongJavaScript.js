

var id_text_search = [];
var json_cookie = getCookie('search_his');
if (json_cookie != null && json_cookie.trim() !== "") {
    var json_cookie_ar = JSON.parse(json_cookie);
    id_text_search = json_cookie_ar;
}

search_word_history();

document.getElementById('search-input').addEventListener('input', function (e) {
    search_word();

});

document.addEventListener('click', function (event) {
    var listSearch = document.getElementById('list_search');
    var pSearchWrap = document.querySelector('.search-box');
    var searchInput = document.getElementById('search-input');
    var wordItem = event.target.closest('.word_item');
    var search_in_other = document.querySelector('.search_in_other');
    var p_close = document.querySelector('.p_close');
    var p_popup = p_close.closest('.p_popup');
    var black_cover_click = document.querySelector('.black_cover');
    var p_popup_active = p_close.closest('.p_popup.p_active');

    if (wordItem !== null && wordItem.contains(event.target)) {
        if (listSearch.classList.contains('p_active')) {
            listSearch.classList.remove('p_active');
        }

        search_word_detail(wordItem, wordItem.getAttribute("data-id-word"));

        search_word_history();
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
        if (p_popup.classList.contains('p_active')) {
            p_popup.classList.remove('p_active');
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
            var newAnchorHTML = '<a href="javascript:void(0)" class="p_empty  p_flex_start">Bạn đan bỏ trống</a>';

            // Thay thế nội dung HTML của phần tử .p_drop_down
            pDropDownElement.innerHTML = newAnchorHTML;
            listSearch.classList.remove('p_active');
        } else {

            console.log('Không có phần tử con trong #list_search');
        }
    }
}


function search_word_detail(target,id_search) {
    var searchInputValue = target.querySelector('.word_text').textContent;
    var selectedLang = document.querySelector('.p_select_lang option:checked').value;
    var selectedLangTrans = document.querySelector('.p_select_lang_trans option:checked').value;
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

    id_text_search.push(search_his);

    var t = JSON.stringify(id_text_search);

    document.cookie = 'search_his=' + t + ';expires=' + dformat + '';

    if (searchInputValue != '') {
        var xhr = new XMLHttpRequest();
        var url = '/Home/Search_result_detail';

        xhr.open('GET', url + '?text=' + encodeURIComponent(searchInputValue) + '&lang=' + encodeURIComponent(selectedLang) + '&lang_tran=' + encodeURIComponent(selectedLangTrans), true);

        xhr.onreadystatechange = function () {
            if (xhr.readyState === 4 && xhr.status === 200) {
                document.getElementById('result').innerHTML = xhr.responseText;
            }
        };

        xhr.send();
    }
}

function search_word_history() {
    var search_history_div = document.querySelector('.search_history');
    var ids = [];

    if (id_text_search.length >0) {
        id_text_search.forEach(function (item) {
            var id = item.id;
            ids.push(id)
        });

        var jsonData = JSON.stringify(ids);
        console.log(jsonData);
        var xhr = new XMLHttpRequest();
        var url = '/Home/Item_history';
        xhr.open('GET', url + '?idList=' + encodeURIComponent(jsonData), true);

        xhr.onreadystatechange = function () {
            if (xhr.readyState === 4 && xhr.status === 200) {
                search_history_div.innerHTML = xhr.responseText;
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