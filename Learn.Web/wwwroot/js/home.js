// When the user scrolls the page, execute myFunction
window.onscroll = function () { myFunction() };

// Get the navbar
var navbar = document.getElementById("navbar");

// Get the offset position of the navbar
var sticky = navbar.offsetTop;

// Add the sticky class to the navbar when you reach its scroll position. Remove "sticky" when you leave the scroll position
function myFunction() {
	if (window.pageYOffset >= sticky) {
		navbar.classList.add("sticky")
	} else {
		navbar.classList.remove("sticky");
	}
(function ($) {
	var productCarousel_1 = '#pl-1',
		productCarousel_2 = '#pl-2',
		productCarousel_3 = '#pl-3',
		productCarousel_4 = '#pl-4',
		productCarousel_5 = '#pl-5';

	var defaults = {
		items: 4,
		itemWidth: 300,
		itemsDesktop: [1260, 3],
		itemsTablet: [930, 2],
		itemsMobile: [620, 1],
		navigation: true,
		navigationText: false
	}

	$(productCarousel_1).owlCarousel(defaults);
	$(productCarousel_2).owlCarousel(defaults);
	$(productCarousel_3).owlCarousel(defaults);
	$(productCarousel_4).owlCarousel(defaults);
	$(productCarousel_5).owlCarousel(defaults);

	function nextSlide(e) {
		e.preventDefault();
		e.data.owlObject.next();
	}

	function prevSlide(e) {
		e.preventDefault();
		e.data.owlObject.prev();
	}

	function registerCarousels(carousels) {
		for(var i=0; i<carousels.length; i++) {
			var id = carousels[i],
				owl = $(id).data('owlCarousel');

			$(id).parent().find('.slide-control.right').bind('click', {owlObject: owl}, nextSlide);
			$(id).parent().find('.slide-control.left').bind('click', {owlObject: owl}, prevSlide);
		}
	}

	var carousels = [ productCarousel_1, productCarousel_2, productCarousel_3, productCarousel_4, productCarousel_5 ];
	registerCarousels(carousels);
})(jQuery);

// When the user scrolls the page, execute myFunction
window.onscroll = function () { myFunction() };

// Get the header
var header = document.getElementById("menu-head");

var headerreg = document.getElementById("menu-reg");
var headerlogo = document.getElementById("menu-logo");


// Get the offset position of the navbar
var sticky = header.offsetTop;
/*var hidden = headerlogo.hidden;*/
// Add the sticky class to the header when you reach its scroll position. Remove "sticky" when you leave the scroll position
function myFunction() {
	if (window.pageYOffset > sticky) {
		header.classList.add("sticky");
		headerreg.classList.remove("hidden");
		headerlogo.classList.remove("hidden");

		headerlogo.show("5s");
		

	} else {
		header.classList.remove("sticky");
		headerreg.classList.add("hidden");
		headerlogo.classList.add("hidden");
		headerlogo.hide(2000);
		
	
	}
}