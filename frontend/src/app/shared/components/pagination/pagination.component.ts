import { Component, Input, Output, EventEmitter, OnChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
    selector: 'app-pagination',
    standalone: true,
    imports: [CommonModule, FormsModule],
    templateUrl: './pagination.component.html',
    styleUrls: ['./pagination.component.css']
})
export class PaginationComponent implements OnChanges {
    @Input() page = 1;
    @Input() pageSize = 10;
    @Input() totalPages = 1;
    @Input() totalCount = 0;
    @Input() pageSizeOptions: number[] = [10, 25, 50, 100];

    @Output() pageChange = new EventEmitter<number>();
    @Output() pageSizeChange = new EventEmitter<number>();

    pages: (number | '...')[] = [];

    ngOnChanges(): void {
        this.buildPages();
    }

    buildPages(): void {
        const total = this.totalPages;
        const cur = this.page;
        const result: (number | '...')[] = [];

        if (total <= 7) {
            for (let i = 1; i <= total; i++) result.push(i);
        } else {
            result.push(1);
            if (cur > 3) result.push('...');
            const start = Math.max(2, cur - 1);
            const end = Math.min(total - 1, cur + 1);
            for (let i = start; i <= end; i++) result.push(i);
            if (cur < total - 2) result.push('...');
            result.push(total);
        }

        this.pages = result;
    }

    goTo(p: number | '...'): void {
        if (p === '...' || p === this.page) return;
        this.pageChange.emit(p as number);
    }

    onPageSizeChange(event: Event): void {
        const size = +(event.target as HTMLSelectElement).value;
        this.pageSizeChange.emit(size);
    }

    isNumber(v: number | '...'): v is number { return typeof v === 'number'; }

    get from(): number { return this.totalCount === 0 ? 0 : (this.page - 1) * this.pageSize + 1; }
    get to(): number   { return Math.min(this.page * this.pageSize, this.totalCount); }
}
