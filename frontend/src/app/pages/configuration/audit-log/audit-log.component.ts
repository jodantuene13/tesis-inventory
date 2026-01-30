import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuditService, AuditLog } from '../../../services/audit.service';

@Component({
    selector: 'app-audit-log',
    standalone: true,
    imports: [CommonModule, FormsModule],
    templateUrl: './audit-log.component.html',
    styleUrls: ['./audit-log.component.css']
})
export class AuditLogComponent implements OnInit {
    logs: AuditLog[] = [];

    // Filters
    fromDate: string = '';
    toDate: string = '';
    executorName: string = '';
    actionType: string = '';

    constructor(private auditService: AuditService) { }

    ngOnInit(): void {
        this.search();
    }

    search(): void {
        this.auditService.getLogs(
            this.fromDate || undefined,
            this.toDate || undefined,
            this.executorName || undefined,
            this.actionType || undefined
        ).subscribe(data => {
            console.log('Audit Logs:', data);
            this.logs = data;
        });
    }

    clearFilters(): void {
        this.fromDate = '';
        this.toDate = '';
        this.executorName = '';
        this.actionType = '';
        this.search();
    }

    getActionClass(action: string): string {
        switch (action) {
            case 'CREATE': return 'bg-green-100 text-green-800';
            case 'UPDATE': return 'bg-blue-100 text-blue-800';
            case 'DELETE': return 'bg-red-100 text-red-800';
            case 'CHANGE_STATUS': return 'bg-yellow-100 text-yellow-800';
            case 'CHANGE_PASSWORD': return 'bg-purple-100 text-purple-800';
            default: return 'bg-gray-100 text-gray-800';
        }
    }

    // Modal Logic
    isModalOpen: boolean = false;
    diffOld: any = null;
    diffNew: any = null;

    openDiff(log: AuditLog): void {
        if (!log.targetUserSnapshot) return;

        try {
            const snapshot = JSON.parse(log.targetUserSnapshot);
            this.diffOld = snapshot.old;
            this.diffNew = snapshot.new;
            this.isModalOpen = true;
        } catch (e) {
            console.error('Error parsing snapshot', e);
        }
    }

    closeModal(): void {
        this.isModalOpen = false;
        this.diffOld = null;
        this.diffNew = null;
    }

    getObjectKeys(obj: any): string[] {
        return obj ? Object.keys(obj) : [];
    }
}
