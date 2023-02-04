(function () {
    /*
    * @param { Element } arrowsEl
    * */
    function toggleChevrons(arrowsEl) {
        const currentArrow = $(arrowsEl).find('i.toggle-icon')[0];
        currentArrow?.classList.toggle('bi-chevron-down');
        currentArrow?.classList.toggle('bi-chevron-up');
    }
    document.addEventListener("DOMContentLoaded", function () {
        document.querySelectorAll('.sidebar .nav-link').forEach(function (element) {

            element.addEventListener('click', function (e) {

                let nextEl = element.nextElementSibling;
                let parentEl = element.parentElement;

                if (nextEl) {
                    e.preventDefault();
                    let mycollapse = new bootstrap.Collapse(nextEl);

                    if (nextEl.classList.contains('show')) {
                        mycollapse.hide();
                    } else {
                        mycollapse.show();
                        // find other submenus with class=show
                        const opened_submenu = parentEl.parentElement.querySelector('.submenu.show');
                        // if it exists, then close all of them
                        if (opened_submenu) {
                            new bootstrap.Collapse(opened_submenu);
                        }
                    }
                    toggleChevrons(element);
                }
            }); // addEventListener
        }) // forEach
        const activeElement = document.querySelector('[data-active=True]');
        //check if active element is a child
        if (activeElement?.parentElement?.classList.contains('submenu-nav-item')) {
            //expand submenu
            const submenu = activeElement.closest('.submenu');
            submenu?.previousElementSibling?.click();
        }
        // add active class to parent element
        activeElement?.parentElement?.classList.toggle('active');
        
    });
// DOMContentLoaded  end    
})()
