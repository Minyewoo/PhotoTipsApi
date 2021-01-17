using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PhotoTips.Core.Models;
using PhotoTips.Core.Repositories;

namespace PhotoTips.Infrastructure.Repositories
{
    public class SubmissionEfRepository : ISubmissionRepository
    {
        private readonly PhotoTipsDbContext _context;

        public SubmissionEfRepository(PhotoTipsDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyCollection<Submission>> Get(CancellationToken cancellationToken)
        {
            return await _context.Submissions.ToListAsync(cancellationToken: cancellationToken);
        }

        public async Task<IReadOnlyCollection<Submission>> Get(int? skip, int? count,
            CancellationToken cancellationToken)
        {
            return skip.HasValue && count.HasValue
                ? await _context.Submissions.Skip(skip.Value).Take(count.Value).ToListAsync(cancellationToken)
                : await Get(cancellationToken);
        }

        public async Task<Submission> Get(string id, CancellationToken cancellationToken)
        {
            return await _context.Submissions.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<IReadOnlyCollection<Submission>> Get(User user, CancellationToken cancellationToken)
        {
            return await _context.Submissions.Where(x => x.Submitter.Id == user.Id)
                .ToListAsync(cancellationToken: cancellationToken);
        }

        public async Task<IReadOnlyCollection<Submission>> GetChecking(CancellationToken cancellationToken)
        {
            return await _context.Submissions.Where(x => x.Status == Submission.SubmissionStatus.Checking)
                .ToListAsync(cancellationToken: cancellationToken);
        }

        public async Task<Submission> Create(Submission submission, CancellationToken cancellationToken)
        {
            submission.Id = Guid.NewGuid().ToString();
            var createdSubmission = await _context.Submissions.AddAsync(submission, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return createdSubmission?.Entity;
        }

        public async Task Update(Submission submission, CancellationToken cancellationToken)
        {
            _context.Submissions.Update(submission);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task Remove(Submission submission, CancellationToken cancellationToken)
        {
            _context.Submissions.Remove(submission);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task Remove(string id, CancellationToken cancellationToken)
        {
            var submission = await Get(id, cancellationToken);
            if (submission != null)
            {
                _context.Submissions.Remove(submission);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}