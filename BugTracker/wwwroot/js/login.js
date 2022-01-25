(function ($) {

	"use strict";

	var fullHeight = function () {

		$('.js-fullheight').css('height', $(window).height()-95);
		$(window).resize(function () {
			$('.js-fullheight').css('height', $(window).height()-95);
		});

	};
	fullHeight();

	$(".toggle-password").click(function () {

		$(this).toggleClass("fa-eye fa-eye-slash");
		var input = $($(this).attr("toggle"));
		if (input.attr("type") == "password") {
			input.attr("type", "text");
		} else {
			input.attr("type", "password");
		}
	});

})(jQuery);
