export class CollectorConfiguration {
    constructor(
        public id: string,
        public displayName: string,
        public schedulerAgentId: string,
        public collectors: [],
        public notifications: []
    ) { }
}
