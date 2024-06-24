//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v14.0.8.0 (NJsonSchema v11.0.1.0 (Newtonsoft.Json v13.0.0.0)) (http://NSwag.org)
// </auto-generated>
//----------------------

/* tslint:disable */
/* eslint-disable */
// ReSharper disable InconsistentNaming

import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { Injectable, Inject, Optional, InjectionToken } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http';

export const API_BASE_URL = new InjectionToken<string>('API_BASE_URL');

export interface IAstronautDutyClient {
    getAstronautDutiesByName(name: string): Observable<GetAstronautDutiesByNameResult>;
    createAstronautDuty(request: CreateAstronautDuty): Observable<CreateAstronautDutyResult>;
}

@Injectable({
    providedIn: 'root'
})
export class AstronautDutyClient implements IAstronautDutyClient {
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl ?? "";
    }

    getAstronautDutiesByName(name: string): Observable<GetAstronautDutiesByNameResult> {
        let url_ = this.baseUrl + "/AstronautDuty/{name}";
        if (name === undefined || name === null)
            throw new Error("The parameter 'name' must be defined.");
        url_ = url_.replace("{name}", encodeURIComponent("" + name));
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processGetAstronautDutiesByName(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetAstronautDutiesByName(response_ as any);
                } catch (e) {
                    return _observableThrow(e) as any as Observable<GetAstronautDutiesByNameResult>;
                }
            } else
                return _observableThrow(response_) as any as Observable<GetAstronautDutiesByNameResult>;
        }));
    }

    protected processGetAstronautDutiesByName(response: HttpResponseBase): Observable<GetAstronautDutiesByNameResult> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (response as any).error instanceof Blob ? (response as any).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
            let result200: any = null;
            result200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver) as GetAstronautDutiesByNameResult;
            return _observableOf(result200);
            }));
        } else if (status === 400) {
            return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
            let result400: any = null;
            result400 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver) as BaseResponse;
            return throwException("A server side error occurred.", status, _responseText, _headers, result400);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf(null as any);
    }

    createAstronautDuty(request: CreateAstronautDuty): Observable<CreateAstronautDutyResult> {
        let url_ = this.baseUrl + "/AstronautDuty";
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(request);

        let options_ : any = {
            body: content_,
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Content-Type": "application/json",
                "Accept": "application/json"
            })
        };

        return this.http.request("post", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processCreateAstronautDuty(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processCreateAstronautDuty(response_ as any);
                } catch (e) {
                    return _observableThrow(e) as any as Observable<CreateAstronautDutyResult>;
                }
            } else
                return _observableThrow(response_) as any as Observable<CreateAstronautDutyResult>;
        }));
    }

    protected processCreateAstronautDuty(response: HttpResponseBase): Observable<CreateAstronautDutyResult> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (response as any).error instanceof Blob ? (response as any).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
            let result200: any = null;
            result200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver) as CreateAstronautDutyResult;
            return _observableOf(result200);
            }));
        } else if (status === 400) {
            return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
            let result400: any = null;
            result400 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver) as BaseResponse;
            return throwException("A server side error occurred.", status, _responseText, _headers, result400);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf(null as any);
    }
}

export interface IPersonClient {
    getPeople(): Observable<GetPeopleResult>;
    createPerson(name: string): Observable<CreatePersonResult>;
    getPersonByName(name: string): Observable<GetPersonByNameResult>;
}

@Injectable({
    providedIn: 'root'
})
export class PersonClient implements IPersonClient {
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl ?? "";
    }

    getPeople(): Observable<GetPeopleResult> {
        let url_ = this.baseUrl + "/Person";
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processGetPeople(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetPeople(response_ as any);
                } catch (e) {
                    return _observableThrow(e) as any as Observable<GetPeopleResult>;
                }
            } else
                return _observableThrow(response_) as any as Observable<GetPeopleResult>;
        }));
    }

    protected processGetPeople(response: HttpResponseBase): Observable<GetPeopleResult> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (response as any).error instanceof Blob ? (response as any).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
            let result200: any = null;
            result200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver) as GetPeopleResult;
            return _observableOf(result200);
            }));
        } else if (status === 400) {
            return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
            let result400: any = null;
            result400 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver) as BaseResponse;
            return throwException("A server side error occurred.", status, _responseText, _headers, result400);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf(null as any);
    }

    createPerson(name: string): Observable<CreatePersonResult> {
        let url_ = this.baseUrl + "/Person";
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(name);

        let options_ : any = {
            body: content_,
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Content-Type": "application/json",
                "Accept": "application/json"
            })
        };

        return this.http.request("post", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processCreatePerson(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processCreatePerson(response_ as any);
                } catch (e) {
                    return _observableThrow(e) as any as Observable<CreatePersonResult>;
                }
            } else
                return _observableThrow(response_) as any as Observable<CreatePersonResult>;
        }));
    }

    protected processCreatePerson(response: HttpResponseBase): Observable<CreatePersonResult> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (response as any).error instanceof Blob ? (response as any).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
            let result200: any = null;
            result200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver) as CreatePersonResult;
            return _observableOf(result200);
            }));
        } else if (status === 400) {
            return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
            let result400: any = null;
            result400 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver) as BaseResponse;
            return throwException("A server side error occurred.", status, _responseText, _headers, result400);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf(null as any);
    }

    getPersonByName(name: string): Observable<GetPersonByNameResult> {
        let url_ = this.baseUrl + "/Person/{name}";
        if (name === undefined || name === null)
            throw new Error("The parameter 'name' must be defined.");
        url_ = url_.replace("{name}", encodeURIComponent("" + name));
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processGetPersonByName(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetPersonByName(response_ as any);
                } catch (e) {
                    return _observableThrow(e) as any as Observable<GetPersonByNameResult>;
                }
            } else
                return _observableThrow(response_) as any as Observable<GetPersonByNameResult>;
        }));
    }

    protected processGetPersonByName(response: HttpResponseBase): Observable<GetPersonByNameResult> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (response as any).error instanceof Blob ? (response as any).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
            let result200: any = null;
            result200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver) as GetPersonByNameResult;
            return _observableOf(result200);
            }));
        } else if (status === 400) {
            return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
            let result400: any = null;
            result400 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver) as BaseResponse;
            return throwException("A server side error occurred.", status, _responseText, _headers, result400);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf(null as any);
    }
}

export interface BaseResponse {
    success?: boolean;
    message?: string;
    responseCode?: number;
}

export interface GetAstronautDutiesByNameResult extends BaseResponse {
    person?: PersonAstronaut | undefined;
    astronautDuties?: AstronautDuty[];
}

export interface PersonAstronaut {
    personId?: number;
    name?: string;
    currentRank?: string;
    currentDutyTitle?: string;
    careerStartDate?: Date | undefined;
    careerEndDate?: Date | undefined;
}

export interface AstronautDuty {
    id?: number;
    personId?: number;
    rank?: string;
    dutyTitle?: string;
    dutyStartDate?: Date;
    dutyEndDate?: Date | undefined;
    person?: Person | undefined;
}

export interface Person {
    id?: number;
    name?: string;
    astronautDetail?: AstronautDetail | undefined;
    astronautDuties?: AstronautDuty[];
}

export interface AstronautDetail {
    id?: number;
    personId?: number;
    currentRank?: string;
    currentDutyTitle?: string;
    careerStartDate?: Date;
    careerEndDate?: Date | undefined;
    person?: Person | undefined;
}

export interface CreateAstronautDutyResult extends BaseResponse {
    id?: number | undefined;
}

export interface CreateAstronautDuty {
    name?: string;
    rank?: string;
    dutyTitle?: string;
    dutyStartDate?: Date;
}

export interface GetPeopleResult extends BaseResponse {
    people?: PersonAstronaut[];
}

export interface GetPersonByNameResult extends BaseResponse {
    person?: PersonAstronaut | undefined;
}

export interface CreatePersonResult extends BaseResponse {
    id?: number;
}

export class ApiException extends Error {
    override message: string;
    status: number;
    response: string;
    headers: { [key: string]: any; };
    result: any;

    constructor(message: string, status: number, response: string, headers: { [key: string]: any; }, result: any) {
        super();

        this.message = message;
        this.status = status;
        this.response = response;
        this.headers = headers;
        this.result = result;
    }

    protected isApiException = true;

    static isApiException(obj: any): obj is ApiException {
        return obj.isApiException === true;
    }
}

function throwException(message: string, status: number, response: string, headers: { [key: string]: any; }, result?: any): Observable<any> {
    if (result !== null && result !== undefined)
        return _observableThrow(result);
    else
        return _observableThrow(new ApiException(message, status, response, headers, null));
}

function blobToText(blob: any): Observable<string> {
    return new Observable<string>((observer: any) => {
        if (!blob) {
            observer.next("");
            observer.complete();
        } else {
            let reader = new FileReader();
            reader.onload = event => {
                observer.next((event.target as any).result);
                observer.complete();
            };
            reader.readAsText(blob);
        }
    });
}