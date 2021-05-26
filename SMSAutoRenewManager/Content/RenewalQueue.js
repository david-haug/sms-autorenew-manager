/*
RenewalQueue.aspx Javascript
*/

        
        function getSelectedIds(){
           //should be for each here...add to selectedIds
           $('#checkedIds').val('');
           //this below not working:
           
           $("input[type=checkbox][checked]").each( 
            function() { 
               $('#checkedIds').val($('#checkedIds').val() + $(this).val() + ',');
            } 
            );
           
 
        }


        //********
        function getRenewalDetail(subscriptionid){
            
            //use timestamp to prevent cached response
            var currentTime = new Date()
            var ts = currentTime.getSeconds() + '_' + currentTime.getMilliseconds();
            var paymentMethod = '';
            
            //get detail with .ajax
            $.ajax({
              url: 'ajax/renewalrecorddetail.aspx',
	          data: 'id=' + subscriptionid + '&ts='+ ts,
	          datatype: 'text',
	          contentType: "application/json; charset=utf-8",
              beforeSend: function() {  clearDetail(); $('#detailThrobber').show(); },
              complete: function() { $('#detailThrobber').hide(); },
              success: function(json){
                    //populate the order type and carrier
                    $('#_subscriptionIDLabel').html(subscriptionid);
                    $('#selectedSubID').val(subscriptionid)
                    $('#_subscriberLocationLabel').html(json.details[0].subscriber.locationname);
                    $('#_subscriberFullNameLabel').html(json.details[0].subscriber.fullname);
                    
                    $('#_orderTypeSelect').val(json.details[0].ordertype);
                    $('#_carrierSelect').val(json.details[0].carriername);
                    $('#_termsSelect').val(json.details[0].carrierterm);
                    $('#_paymentMethodLabel').html(json.details[0].paymentmethod);
                    paymentMethod = json.details[0].paymentmethod;
                    if(paymentMethod!='Purchase Order')
                    {
                         $('#changePaymentLink').css('visibility','visible');
                    }
                    else
                    {
                        $('#changePaymentLink').css('visibility','hidden');
                    }
                    
                    //loop through the items
                    $.each(json.details, function(i,detail){
                        
                        var excludeMarkup = '';
                        if(detail.excludefrominvoice == "True")
                        {
                            excludeMarkup = ' checked="yes"'
                        };
                    
                            $('#detail').append('<div class="detailItem"><table id="' + detail.alissale.transactiondetailid + '"><tr class="detailItemHeader"><td></td><td>Item</td><td>Qty</td><td colspan="2">Unit Price</td></tr>' + 
                        '<tr><td class="detailItemHeader">Previous</td><td>' + detail.alissale.name + '</td><td>' + detail.alissale.quantity + '</td><td colspan="2">' + detail.alissale.price + '</td></tr>' + 
                       '<tr><td class="detailItemHeader">New</td><td><input type="text" class="detailTextBox" value="' + detail.renewalproduct.name + '"/></td><td><input type="text" class="detailTextBoxShort" value="' + detail.alissale.quantity + '"/></td><td><input type="text" class="detailTextBoxShort" value="' + detail.renewalproduct.price + '"></td><td><input type="checkbox"' + excludeMarkup + '/><span class="detailExclude">Exclude from invoice</span></td></tr></table></div>');
                      
                    });
                   
                }
                //--end success
            }); //--end ajax
            


        };                                                                                                    
        
        function clearDetail()
        {
            $('#detail').html('');
            $('#_subscriberLocationLabel').html('');
            $('#_subscriptionIDLabel').html('');
            $('#_paymentMethodLabel').html('');
            $('#_subscriberFullNameLabel').html('');
         }
        
        
        //********

        $(document).ready(function() {
          //load menu options
          var currentDate = new Date();

          var endWeek = new Date();
          endWeek.setDate(currentDate.getDate()+7);
          var endMonth = new Date(currentDate.getFullYear(), currentDate.getMonth() + 1, 0);
          
          var expiredEndDate = currentDate.getMonth()+1 + '/' + currentDate.getDate() + '/' + currentDate.getFullYear();
          var weekEndDate = endWeek.getMonth()+1 + '/' + endWeek.getDate() + '/' + endWeek.getFullYear();
          var monthEndDate = endMonth.getMonth()+1 + '/' + endMonth.getDate() + '/' + endMonth.getFullYear();

          
          $('#menuExpired').html('<a href="javascript: loadQueueByDate(\'1/1/2000\',\'' + expiredEndDate + '\');">Expired</a>');
          $('#menuWeek').html('<a href="javascript: loadQueueByDate(\'1/1/2000\',\'' + weekEndDate + '\');">Expire This Week</a>');
          $('#menuMonth').html('<a href="javascript: loadQueueByDate(\'1/1/2000\',\'' + monthEndDate + '\');">Expire This Month</a>');
          
 
          
          sortQueue(1);
        });
        //********
        
        $(document).keydown(function(e) {
            cancelArrowKeys(e)
        });
        
        function cancelArrowKeys(e) {  
         try {  
             var e = window.event || e  
  
  
             var key = e.charCode ? e.charCode : e.keyCode ? e.keyCode : 0;  
  
  
             if (key == 38 || key == 40) {  
                 if (key == 38)  
                      moveUp(); 
                  if (key == 40)  
                     moveDown();
  
                 event.returnValue = false;  
            }  
        }  
         catch (Exception) {  
             return false;  
         }  
  
     }  
        
        //********
       function moveDown()
       {
            //find row with subscription id from detail field...
            var subID = $('#_subscriptionIDLabel').html();

            //make sure there is a row below
            if($('table').find('#row' + subID).next('tr').is('tr'))
            {
                var nextSubID = $('table').find('#row' + subID).next('tr').attr("id").replace("row", "");
                //move to next row...
                getRenewalDetail(nextSubID);
                $("#queueDataTable tr").removeClass("dataTableRowSelected");
                $('table').find('#row' + nextSubID).addClass("dataTableRowSelected");
            } 
       };
       //********
      
       function moveUp()
       {
            //find row with subscription id from detail field...
            var subID = $('#_subscriptionIDLabel').html();
            
            //make sure there is a row above
            
           if($('table').find('#row' + subID).prev('tr').hasClass('dataTableCaption')==false)
            {
                //now find row in table with this subID...
                //alert($('table').find('#row' + subID).attr("id"));
                var nextSubID = $('table').find('#row' + subID).prev('tr').attr("id").replace("row", "");
             
            
                //move to next row...
                getRenewalDetail(nextSubID);
                $("#queueDataTable tr").removeClass("dataTableRowSelected");
                $('table').find('#row' + nextSubID).addClass("dataTableRowSelected");
            }
            
            
            

       };
       //********
        
        function saveRenewalDetail()
        {
           //save details...
           
          $('div[class=detailItem] table').each(function(){

           var exclude = false;
           if ($(this).find('td:eq(12) input').is(':checked'))
            {
                exclude = true;
            };
           
           var detailToSave = { 
                "transactionDetailID": $(this).attr('id'), 
                "carriername":$('#_carrierSelect').val(),
                "carrierterm":$('#_termsSelect').val(),
                "ordertype":$('#_orderTypeSelect').val(),
                "paymentmethod":$('#_paymentMethodLabel').html(),
                "productname":$(this).find('td:eq(9) input').val(),
                "quantity":$(this).find('td:eq(10) input').val(),
                "price":$(this).find('td:eq(11) input').val(),
                "excludefrominvoice" : exclude         
            };
           
           var successMsg = "";
           $.post("ajax/renewalrecorddetail.aspx?save=true", detailToSave ,function(data){
            successMsg==data.success;
            },"json");
          
          });
                
          //update corresponding checkbox??
          var sjId = $('#_subscriptionIDLabel').html()
          //$('input[value=' + sjId + ']').attr({checked: 'true', disabled: 'false'}); 
          $('input[value=' + sjId + ']').attr('disabled', false);
          $('input[value=' + sjId + ']').attr({checked: 'true'}); 
          $('#checkedIds').val($('#checkedIds').val() + sjId + ',');
  
        };

        //********
        function sortQueue(col){
        
            var subID = $('#_subscriptionIDLabel').html();
            var startDate = $('#_startDateFilterTextBox').val();
            var endDate = $('#_endDateFilterTextBox').val();

            //use timestamp to prevent cached response
            var currentTime = new Date()
            var ts = currentTime.getSeconds() + '_' + currentTime.getMilliseconds();

             //load queue
            $.ajax({
            url: 'ajax/queuehtmltable.aspx',
	        data: 'date1=' + startDate + '&date2=' + endDate + '&col=' + col + '&ts='+ ts,
	        datatype: 'text',
            beforeSend: function() { $('#renewals').html('');$('#queueThrobber').show(); },
            complete: function() { $('#queueThrobber').hide(); },
            success: function(html){
                //--start success
                $('#renewals').html('');
                $('#renewals').append(html);
                
                if($('table').find('#row' + subID).is('tr'))
                {
                    //subId was found in table, highlight record
                    $('table').find('#row' + subID).addClass("dataTableRowSelected");
                    getRenewalDetail(subID);
                }
                else
                {
                    //highlight first row
                    $('#queueDataTable tr:eq(1)').addClass("dataTableRowSelected");
                    //also need to get renewal detail..need 2nd row id...
    
                    //verify records returned in table or there will be error
                    if ($('#queueDataTable tr:eq(1)').is('tr'))
                    {
                        var firstId = $('#queueDataTable tr:eq(1)').attr("id").replace("row", "");
                        getRenewalDetail(firstId);
                    }
                    else
                    {
                        //no results, clear details as well...
                        clearDetail();
                        //hide throbber
                        $('#detailThrobber').hide();
                    }
    
                }
                
                
                
                $("#queueDataTable tr").click(function() {
                    $("#queueDataTable tr").removeClass("dataTableRowSelected");
                    $(this).addClass("dataTableRowSelected");
                  });
                
                $("input:checkbox").click(function() {
                        $(this).attr('disabled','true');
                        getSelectedIds();
                   });
                
                //for each rowid in selctedId...checkbox
                var idsToCheck =  $('#checkedIds').val().split(",");
                for(i = 0; i < idsToCheck.length; i++){
                    $('input[value=' + idsToCheck[i] + ']').attr('disabled', false);
	                $('input[value=' + idsToCheck[i] + ']').attr({checked: 'true'});
                };
                
                
                //--end success
                }
                
                
            })
            }
              
              
            function loadQueueByDate(startDate,endDate)
            {
                $('#_startDateFilterTextBox').val(startDate);
                $('#_endDateFilterTextBox').val(endDate);
                sortQueue(1);
            }
            
            function changePaymentMethod()
            {
                $('#_paymentMethodLabel').html('Purchase Order');
               $('#changePaymentLink').css('visibility','hidden');
            } 
            
            function closeMessage()
            {
                $('#_messageDiv').html('');
            }
            
            function createMessage(message)
            {
                $('#_messageDiv').html('<div class="msgSuccess"><img src="images/message_success.png" class="msgIcon"/>' + message + '<a href="javascript: closeMessage();" class="msgClose">[close]</a></div>');
            }
