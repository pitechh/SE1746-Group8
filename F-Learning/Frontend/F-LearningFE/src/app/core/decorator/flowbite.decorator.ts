import { initFlowbite } from "flowbite";
import { Subject, concatMap, delay, of } from "rxjs";

// Tạo queue để xử lý Flowbite initialization
let flowbiteQueue = new Subject<any>();

// Xử lý queue với delay 100ms giữa các item
flowbiteQueue.pipe(
    concatMap(item => of(item).pipe(delay(100)))
).subscribe((x) => {
    x();
});

// Decorator chính
export function Flowbite() {
    return function (target: any) {
        // Lưu trữ ngOnInit gốc
        const originalOnInit = target.prototype.ngOnInit;
        
        // Override ngOnInit
        target.prototype.ngOnInit = function () {
            // Gọi ngOnInit gốc nếu tồn tại
            if (originalOnInit) {
                originalOnInit.apply(this);
            }
            // Gọi hàm fix Flowbite
            InitFlowbiteFix();
        };
    };
}

// Hàm xử lý initialization
export function InitFlowbiteFix() {
    flowbiteQueue.next(() => {
        // Lấy tất cả elements trong DOM
        const elements = document.querySelectorAll('*');
        const flowbiteElements: HTMLElement[] = [];
        const initializedElements = Array.from(document.querySelectorAll('[flowbite-initialized]'));

        // Tìm các elements có data- attributes
        for (let i = 0; i < elements.length; i++) {
            const element = elements[i];
            const attributes = element.attributes;

            for (let j = 0; j < attributes.length; j++) {
                const attribute = attributes[j];

                if (attribute.name.startsWith('data-')) {
                    if (!flowbiteElements.includes(element as HTMLElement) && 
                        !initializedElements.find(x => x.isEqualNode(element))) {
                        flowbiteElements.push(element as HTMLElement);
                    }
                }
            }
        }

        // Đánh dấu các elements đã được initialized
        for (let i = 0; i < flowbiteElements.length; i++) {
            flowbiteElements[i].setAttribute('flowbite-initialized', '');
        }

        // Khởi tạo Flowbite
        initFlowbite();

        // Xử lý data attributes
        flowbiteElements.forEach(element => {
            const attributes: { name: string; value: string }[] = Array.from(element.attributes);
            const dataAttributes = attributes.filter(attribute => attribute.name.startsWith('data-'));

            dataAttributes.forEach(attribute => {
                element.setAttribute(attribute.name.replace('data-', 'fb-'), attribute.value);
                element.removeAttribute(attribute.name);
            });
        });
    });
}