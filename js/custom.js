$(document).ready(function() {
	'use strict';
	if (navigator.userAgent.match(/IEMobile\/10\.0/)) {
		var msViewportStyle = document.createElement("style")
		msViewportStyle.appendChild(
			document.createTextNode(
				"@-ms-viewport{width:auto!important}"
			)
		)
		document.getElementsByTagName("head")[0].appendChild(msViewportStyle)
	}
	imagesLoaded(document.body, function(){
		if ($('.no-touch').length) {
			skrollr.init({
				smoothScrolling: false,
				forceHeight: false
			});
		}
	});
	$('#carousel_fade_intro').carousel({
		interval: 6000,
		pause: "false"
	})
	$('#carousel-1, #carousel-2 #carousel-3').carousel({
		interval: false
	})
	$(function(){
		$('#intro .item').css({'height':($(window).height())+'px'});
		$(window).resize(function(){
		$('#intro .item').css({'height':($(window).height())+'px'});
		});
	});
});
$(window).load(function() {
	'use strict';
	if ($('.navbar-toggle:visible').length) {
		$('.navbar a').click(function () { $(".navbar-collapse").collapse("hide") });
	}
	$('.fit-video').fitVids();
	$.localScroll.hash();
	$('.scroll-btn, #more, .hidden-xs, .navbar, .navbar-header, #footer').localScroll({
		target: 'body',
		queue: true,
		duration: 1000,
		hash: false,
		offset: -60,
		easing: 'easeInOutExpo'
	});
	$('.spinner').fadeOut('fast');
	$('.preloader').delay(350).fadeOut('fast');
	$('a[href="#"]').click(function() {
		return false;
	});
});