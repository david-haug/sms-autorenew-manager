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

          
          $('#menuExpired').html('<a href="renewalqueue.aspx?startdate=1/1/2000&enddate=' + expiredEndDate + '">Expired</a>');
          $('#menuWeek').html('<a href="renewalqueue.aspx?startdate=1/1/2000&enddate=' + weekEndDate + '">Expire This Week</a>');
          $('#menuMonth').html('<a href="renewalqueue.aspx?startdate=1/1/2000&enddate=' + monthEndDate + '">Expire This Month</a>');
         
         
        });
        //********