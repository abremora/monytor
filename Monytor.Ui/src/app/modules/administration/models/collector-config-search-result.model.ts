export class CollectorConfigSearchResult {
  constructor(
    public collectorConfigId: string,
    public displayName: string,
    public schedulerAgentId: string,
    public collectorCount: number,
    public notificationCount: number
  ) {}
}
