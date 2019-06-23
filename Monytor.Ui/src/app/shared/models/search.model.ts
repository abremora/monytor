export class Search<TResult> {
  constructor(
    public items: Array<TResult>,
    public page: number,
    public pageSize: number,
    public totalPages: number,
    public totalResults: number
  ) {}
}
