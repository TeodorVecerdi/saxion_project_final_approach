module.exports = class VotingSession {
    constructor(guid, creator, reason, votes) {
        this.guid = guid;
        this.creator = creator;
        this.reason = reason;
        this.votes = votes;
    }

    toJSON() {
        return {
            'guid': this.guid,
            'creator': this.creator,
            'reason': this.reason,
            'votes': this.votes
        }
    }
};