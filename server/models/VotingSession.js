module.exports = class VotingSession {
    constructor(guid, reason, votes) {
        this.guid = guid;
        this.reason = reason;
        this.votes = votes;
    }

    toJSON() {
        return {
            'guid': this.guid,
            'reason': this.reason,
            'votes': this.votes
        }
    }
};