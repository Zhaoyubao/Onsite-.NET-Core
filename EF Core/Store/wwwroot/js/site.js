$(document).ready(function() {
   
    $("input#searchCustomer").keydown( () => setTimeout(customerFilter, 500) )

    function customerFilter() {
        let filter = $("input#searchCustomer").val();
        let data = `customer=${filter}`;
        $.ajax({
            method: "POST",
            url: "/customers/filter",
            data: data,
            success: res => $("table#customers").html(res)
        })
    }

    $("input#searchProduct").keydown( () => setTimeout(productFilter, 500) )

     function productFilter() {
        let filter = $("input#searchProduct").val();
        let data = `product=${filter}`;
        $.ajax({
            method: "POST",
            url: "/products/filter",
            data: data,
            success: res => $("div#products").html(res)
        })
    }

// ===============================================================================
    $("select#orders").change( () =>{
        let id = $("select#orders").val();
        if(id) {
            $.ajax({
                method: "Get",
                url: `/customers/${id}`,
                success: res => $("table#orders").html(res)
            })
        }
    })

})