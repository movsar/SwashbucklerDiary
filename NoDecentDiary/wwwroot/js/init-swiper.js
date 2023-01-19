import '/js/swiper-bundle.min.js';
export function swiperInit(dotNetCallbackRef, callbackMethod, id, index) {
    console.log('Entered initSwiper!');
    let className = "." + id;
    window[id] = new Swiper(className, {
        observer: true,
        observeParents: true,
        observeSlideChildren: true,
        autoHeight: true,//自动高度
        simulateTouch: false,//禁止鼠标模拟
        initialSlide: index,//设定初始化时slide的索引
        on: {
            slideChangeTransitionStart: function () {
                dotNetCallbackRef.invokeMethodAsync(callbackMethod, this.activeIndex);
            },
        }
    });
}
