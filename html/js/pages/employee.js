$(document).ready(function() {
    initsEvents();

    //load dữ liệu
    loadData();


})


/**
 * Thực hiện load dữ liệu lên table
 * Nguyễn Tùng Dương
 */
function loadData() {
    //gọi API lấy dữ liệu
    console.log("CALL AJAX !!!!");
    $.ajax({
        type: "GET",
        async: false,
        url: "http://localhost:20029/api/Employees",
        success: function(res) {
            console.log("GET DATA DONE !!!");
            $("table#tableListEmployee tbody").empty();
            //xử lí dữ liệu hướng đối tượng
            let ths = $("table#tableListEmployee thead th");




            for (const emp of res) {
                var trElement = $(`<tr></tr>`);
                for (const th of ths) {
                    //lấy ra propValue
                    const propValue = $(th).attr("propValue");
                    const format = $(th).attr("format");



                    //lấy giá trị tương ứng của propValue
                    let value = emp[propValue];

                    switch (format) {
                        case "date":
                            value = formatDate(value);
                            break;
                        case "gender":
                            value = formatGender(value);
                            break;
                        default:
                            break;
                    }

                    //tạo th HTML
                    let thHTML = `<th>${value}</th>`;

                    //đẩy vào trHTML
                    trElement.append(thHTML);

                }
                $("table#tableListEmployee tbody").append(trElement);

                // return;
                // const employeeCode = emp.employeeCode;
                // const employeeName = emp.employeeName;

                // let dateOfBirth = emp.dateOfBirth;
                // dateOfBirth = formatDate(dateOfBirth);

                // let gender = emp.gender;
                // Gender = formatGender(Gender);

                // const identityNumber = emp.identityNumber;
                // const positionName = emp.positionName;
                // const departmentName = emp.departmentName;
                // const accountNumber = emp.accountNumber;
                // const bank = emp.bank;
                // const bankBranch = emp.bankBranch;


                // build các tr html tương ứng
                // let trHTML = `<tr>
                //             <td><input type="checkbox" name="" id=""></td>
                //             <td>${employeeCode}</td>
                //             <td>${employeeName}</td>
                //             <td>${gender}</td>
                //             <td>${dateOfBirth}</td>
                //             <td>${identityNumber}</td>
                //             <td>${positionName}</td>
                //             <td>${departmentName}</td>
                //             <td>${accountNumber}</td>
                //             <td>${bank}</td>
                //             <td>${bankBranch}</td>
                //             <td>
                //                 <select class="combobox" name="comboBox" id="comboBox">
                //                     <option value="1">Sửa</option>
                //                     <option value="2">Nhân bản</option>
                //                     <option value="3">Ngừng sử dụng</option>
                //                 </select>
                //             </td>
                //             </tr>`;

                // thực hiện append các tr html
                // $("table#tableListEmployee tbody").append(trHTML);
            }
        }
    });
}

//định dạng giới tính
function formatGender(Gender) {
    try {
        if (Gender == 0) {
            return `Khác`;
        }
        if (Gender == 1) {
            return `Nam`;
        }
        if (Gender == 2) {
            return `Nữ`;
        }
    } catch (error) {
        console.log(error);
    }
}

//định dạng hiển thị ngày tháng năm
function formatDate(date) {
    try {
        if (date) {
            date = new Date(date);
            //lấy ngày
            dateValue = date.getDate();
            //lấy tháng
            month = date.getMonth() + 1;
            //lấy năm
            year = date.getFullYear();

            return `${dateValue}/${month}/${year}`;
        } else {
            return "";
        }
    } catch (error) {
        console.log(error);
    }
}

/**Tạo các sự kiện
 * Nguyễn Tùng Dương
 */

function initsEvents() {
    //gán sự kiện nhấn nút refresh thì reload
    $(".btn-refresh").click(function() {
        location.reload(true);
    });

    //gán sự kiện khi nhấn vào button thêm mới nhân viên:
    $("#btnAdd").click(function() {
        $("#dlInfor").show();
    })

    //nhấn close ẩn form chi tiết:
    $("#dialog__close-employee").click(function() {
        $(this).parents(".dlInfor").hide();
    })

    //gán sự kiện khi nhấn đúp chuột vào 1 (tr) nhân viên thì hiển thị chi tiết thông tin:


    //nhấn button xóa thì cảnh báo xóa:



}