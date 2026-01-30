import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface AuditLog {
    id: number;
    entityId: number;
    entityType: string;
    action: string;
    timestamp: string; // ISO date
    details: string;
    executorId?: number;
    executorName: string;
    executorEmail: string;
    executorRole: string;
    executorSede: string;
    targetUserSnapshot?: string; // JSON with old/new state
}

@Injectable({
    providedIn: 'root'
})
export class AuditService {
    private apiUrl = 'http://localhost:5139/api/audit'; // Consistent with UserService

    constructor(private http: HttpClient) { }

    getLogs(fromDate?: string, toDate?: string, executorName?: string, actionType?: string): Observable<AuditLog[]> {
        let params = new HttpParams();
        if (fromDate) params = params.set('fromDate', fromDate);
        if (toDate) params = params.set('toDate', toDate);
        if (executorName) params = params.set('executorName', executorName);
        if (actionType) params = params.set('actionType', actionType);

        return this.http.get<AuditLog[]>(this.apiUrl, { params });
    }
}
