/*! Call Tab */
$(document).ready(function(){
	
	$('ul.tabs li').click(function(){
		var tab_id = $(this).attr('data-tab');

		$('ul.tabs li').removeClass('current');
		$('.tab-content').removeClass('current');

		$(this).addClass('current');
		$("#"+tab_id).addClass('current');
	})

})

/*! Call WOW */
 wow = new WOW(
		{
		  animateClass: 'animated',
		  offset:       100,
		  mobile:       false
		}
	  );
	wow.init();

/*! Call sticky */
$(window).load(function(){
	  $("#header").sticky({ topSpacing: 0 });
	});


/*! Call slide-ks */

jQuery(document).ready(function($) {

	'use strict';

		$(".slide-project").owlCarousel({
			slideSpeed : 200,
			items : 4,
			itemsCustom : false,
			itemsDesktop : [1199, 4],
			itemsDesktopSmall : [979, 3],
			itemsTablet : [768, 2],
			itemsTabletSmall : false,
			itemsMobile : [479, 1],
			autoPlay: true,
			stopOnHover: true,
			addClassActive: true,
			autoHeight: true,
			responsive: true,
			navigation: true,
			pagination : false,
			navigationText: ["",""],
		});
 
});

/*! Call slide-kh */

jQuery(document).ready(function($) {

	'use strict';

		$(".slide-kh").owlCarousel({
			slideSpeed : 200,
			items : 2,
			itemsCustom : false,
			itemsDesktop : [1199, 2],
			itemsDesktopSmall : [979, 2],
			itemsTablet : [768, 2],
			itemsTabletSmall : false,
			itemsMobile : [479, 1],
			autoPlay: true,
			stopOnHover: true, 
			responsive: true,
			navigation: true,
			pagination : true,
			navigationText: ["",""], 
		});
 
});

/*! Call slide-video */

jQuery(document).ready(function($) {

	'use strict';

		$(".slide-func").owlCarousel({
			slideSpeed : 200,
			items : 4,
			itemsCustom : false,
			itemsDesktop : [1199, 4],
			itemsDesktopSmall : [979, 3],
			itemsTablet : [768, 2],
			itemsTabletSmall : false,
			itemsMobile : [479, 1],
			autoPlay: true,
			stopOnHover: true,
			responsive: true,
			navigation: true,
			pagination : false,
			navigationText: ["",""], 
		});
});		



/**  Tool tip **/
$(document).ready(function () {
  $('.tooltip-right').tooltip({
    placement: 'right',
    viewport: {selector: 'body', padding: 3}
  })
   $('.tooltip-left').tooltip({
    placement: 'left',
    viewport: {selector: 'body', padding: 3}
  })
  $('.tooltip-top').tooltip({
    placement: 'top',
    viewport: {selector: 'body', padding: 3}
  })
  $('.tooltip-bottom').tooltip({
    placement: 'bottom',
    viewport: {selector: 'body', padding: 3}
  })
 
})
			  

/**  ve top **/

jQuery(document).ready(function($){
	// browser window scroll (in pixels) after which the "back to top" link is shown
	var offset = 300,
		//browser window scroll (in pixels) after which the "back to top" link opacity is reduced
		offset_opacity = 1200,
		//duration of the top scrolling animation (in ms)
		scroll_top_duration = 700,
		//grab the "back to top" link
		$back_to_top = $('.cd-top');

	//hide or show the "back to top" link
	$(window).scroll(function(){
		( $(this).scrollTop() > offset ) ? $back_to_top.addClass('cd-is-visible') : $back_to_top.removeClass('cd-is-visible cd-fade-out');
		if( $(this).scrollTop() > offset_opacity ) { 
			$back_to_top.addClass('cd-fade-out');
		}
	});

	//smooth scroll to top
	$back_to_top.on('click', function(event){
		event.preventDefault();
		$('body,html').animate({
			scrollTop: 0 ,
		 	}, scroll_top_duration
		);
	});

});			

